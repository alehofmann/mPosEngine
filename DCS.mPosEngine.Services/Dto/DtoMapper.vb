Imports System
Imports DCS.mPosEngine.Data.Dao
Imports DCS.mPosEngine.Services.Dto
Imports DCS.mPosEngine.Core.Domain.Sales
Imports DCS.mPosEngine.Data.Infrastructure
Imports DCS.mPosEngine.Core.Domain.Sales.Payment
Imports DCS.mPosEngine.Core.Domain.ExternalSales
Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.PlaycardBase.Core.PosDomain
Imports NHibernate.Util

Namespace Dto

	Public Class InvalidDtoFieldException
		Inherits ArgumentException
		Private _fieldName As String

		Public Property FieldName() As String
			Get
				Return _fieldName
			End Get
			Set(ByVal value As String)
				_fieldName = value
			End Set
		End Property

		Public Sub New(ByVal message As String)
			MyBase.New(message)
		End Sub
		Public Sub New(ByVal fieldName As String, ByVal message As String)
			MyBase.New("Invalid field data: [" & fieldName & "]." & message)

			_fieldName = fieldName
		End Sub
	End Class

	Public Class InvalidDtoException
		Inherits ApplicationException
		Private _reasonId As Integer
		Public Property ReasonId() As Integer
			Get
				Return _reasonId
			End Get
			Set(ByVal value As Integer)
				_reasonId = value
			End Set
		End Property

		Public Sub New(ByVal reasonId As Integer, ByVal reasonDescription As String)
			MyBase.New(reasonDescription)

			_reasonId = reasonId
		End Sub
	End Class

	Public Class NullCommandDtoException
		Inherits InvalidDtoException

		Public Sub New()
			MyBase.New(1, "Command is null")
		End Sub
	End Class

	Public Class DtoMapper
		Private _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Function GetPaymentData(ByVal payItems As IList(Of PayItemDto)) As PaymentData
            Dim daoFactory As New DCS.PlaycardBase.Data.NHibernateDaoFactory
            Dim currencyRepo As DCS.PlaycardBase.Core.DataInterfaces.ICurrencyDao = daoFactory.GetCurrencyDao
            Dim paymentData As New PaymentData

            If payItems IsNot Nothing Then
                For Each payitem As PayItemDto In payItems
                    Dim currency = currencyRepo.Session.Get(GetType(DCS.PlaycardBase.Core.PosDomain.Currency), payitem.PaymodeId)

                    If currency Is Nothing Then
                        Throw New InvalidDtoFieldException("paymodeId", "Invalid PaymodeId: " & payitem.PaymodeId)
                    End If

                    paymentData.AddPayitem(currency,
                                           payitem.Amount,
                                           payitem.Gratuity,
                                           PaymodeData.GetPaymodeData(currency.PaymodeType,
                                                                      payitem.CreditCardNumber,
                                                                      payitem.CreditCardType,
                                                                      payitem.CreditCardDebugInfo,
                                                                      payitem.CreditCardAuthorizationReference,
                                                                      payitem.PlaycardNumber)
                                           )
                Next
            End If

            Return paymentData
        End Function

        Public Function GetDomainTransactionFrom(ByVal cart As TransactionCartDto, discountsApplied As String) As MPosTransaction
			'Generates a fake command and thus a fake mPosTransaction using only the cart. This is only for getting transaction totals
			'A transaction generated this way should never be paid or commited

			Dim command As New CommitTransactionCommandDto

			command.Cart = cart
			command.OperatorId = 1
			command.PosName = "DUMMY"
			command.DiscountsApplied = discountsApplied

			Return GetDomainTransactionFrom(command)
		End Function

		Public Function GetDomainTransactionFrom(ByVal command As CommitTransactionCommandDto) As MPosTransaction
			Dim cart As TransactionCartDto = command.Cart
			Dim retVal As MPosTransaction
			Dim mPosProduct As DCS.mPosEngine.Core.Domain.Sales.Product
			Dim productData As DCS.PlaycardBase.Core.PosDomain.Product
			Dim externalSystemData As ExternalSystem = Nothing

			_log.Debug("Creating MPosTransaction")
			retVal = New MPosTransaction(command.OperatorId, command.PosName, command.InvoiceNumber, False, New PlaycardBase.Services.DiscountEngine)

			_log.Debug("Analyzing shopping cart data (" & cart.LineItems.Count & " items)")

			For Each lineItemDto As LineitemDto In cart.LineItems
				_log.Debug("Adding lineitem to shopping cart")

                If lineItemDto.ExternalSystemData Is Nothing Then
                    'Si no hay external data lo que viene es el id de mPosProduct

                    _log.Debug("Retrieving mPosProduct id " & lineItemDto.ProductId & " from DB")

                    Dim mPosProductDao As ProductDao = (New NHibernateDaoFactory).GetProductDao

                    mPosProduct = mPosProductDao.GetById(lineItemDto.ProductId)
                    If mPosProduct Is Nothing Then
                        _log.Error("mPosProduct id " & lineItemDto.ProductId & " not found in DB (mPosProducts), throwing an exception")
                        Throw New ApplicationException("Product id " & lineItemDto.ProductId & " not found in DB")
                    End If

                    productData = mPosProduct.ProductData
                Else
                    'Si HAY external data el id que viene es de cnfCashierTransTypes

                    _log.Debug("Retrieving CashierTransTypeId " & lineItemDto.ProductId & " from DB")

					Dim productDao As DCS.PlaycardBase.Core.DataInterfaces.IProductDao = New DCS.PlaycardBase.Data.ProductDao
					productData = productDao.GetById(lineItemDto.ProductId)

					If productData Is Nothing Then
						_log.Error("CashierTransTypeId " & lineItemDto.ProductId & " not found in DB (cnfCashierTransTypes), throwing an exception")
						Throw New ApplicationException("CashierTransTypeId " & lineItemDto.ProductId & " not found in DB (cnfCashierTransTypes)")
					End If

					externalSystemData = GetExternalSystem(lineItemDto.ExternalSystemData)
				End If

				Dim cardInfo As Fulfillment.CardInfo = Nothing
				If lineItemDto.CardNumber > 0 Then
					cardInfo = CardManagerFactory.GetCardManager.GetCardInfo(lineItemDto.CardNumber)
				End If

				retVal.AddLineitem(lineItemDto.UnitPrice, lineItemDto.Quantity, productData, cardInfo, externalSystemData)
			Next

			If Not String.IsNullOrWhiteSpace(command.DiscountsApplied) Then
				_log.Info("Applying discounts to transaction: [" & command.DiscountsApplied & "]")
				Dim discountsIds As New List(Of Integer)
				command.DiscountsApplied.Split(",").ForEach(Sub(x) discountsIds.Add(Convert.ToInt32(x)))
				retVal.ApplyDiscounts(discountsIds)
			End If

			Return retVal
		End Function

		Private Function GetExternalSystem(ByVal data As ExternalSystemDataDto) As ExternalSystem
			Dim transactionExporterDao As ITransactionExporterDao = (New NHibernateDaoFactory).GetTransactionExporterDao
			Dim transactionExporter As ExternalSystem

			transactionExporter = transactionExporterDao.GetById(data.ExternalSystemId)

			If transactionExporter Is Nothing Then
				_log.Error("External system id " & data.ExternalSystemId & " not found in DB")
				Throw New ApplicationException("External system id " & data.ExternalSystemId & " not found in DB, throwing an exception")
			End If

			transactionExporter.Params = data.Params

			Return transactionExporter
		End Function
		Public Function GetProductDtoFrom(ByVal product As DCS.mPosEngine.Core.Domain.Sales.Product, ByVal userCardNumber As Long) As ProductDto
			Dim user As Core.Domain.User
			Dim productDto As ProductDto
			Dim authenticationRequired As Boolean
			Dim posServices As New PosServices

			If product.ForceReenterCredentials Then
				authenticationRequired = True
			Else
				authenticationRequired = (posServices.CheckAccess(product.SecurityActionId, userCardNumber, user).ResultCode = CheckAccessResponseDto.ResultCodesEnum.AccessDenied)
			End If
			productDto = New ProductDto(product.Id, product.ProductData.Name, product.ProductData.Price, product.NeedsQuantity, product.ThumbUrl, product.ProductData.Price)
			productDto.CardRequired = product.ProductData.NeedsCard
			productDto.CardIncluded = product.ProductData.SellsNewCard
			productDto.IsDefaultCardSellProduct = product.IsDefaultCardSellProduct
			productDto.AuthenticationRequired = authenticationRequired
			productDto.SecurityActionId = product.SecurityActionId

			Return productDto
		End Function
	End Class
End Namespace