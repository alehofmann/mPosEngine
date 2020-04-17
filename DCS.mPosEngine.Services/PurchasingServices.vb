Imports System

Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.mPosEngine.Data.Dao
Imports DCS.mPosEngine.Services.Dto
Imports DCS.mPosEngine.Core.DomainServices
Imports DCS.mPosEngine.Core.Domain.Sales
Imports DCS.mPosEngine.Data.Infrastructure
Imports DCS.ProjectBase.Data.NHibernateSessionMgmt
Imports DCS.mPosEngine.Core.Domain.Sales.Payment
Imports DCS.mPosEngine.Services.Payment
Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment

Public Class PurchasingServices
	Private _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	Private _fulfillmentService As FulfillmentService
	'Private _cardFactory As New DCS.PlaycardBase.CardData.CardFactory
	Private ReadOnly _paymentService As PaymentService
	Private _treasuryEnabled As Boolean
	Private _treasuryEngine As ITreasuryEngine
    Private _httpExternalSystemConnector As New HttpExternalSystemConnector

    Private Const _VoucherHardKey As String = "cachafaz"

    Public Sub New(paymentService As PaymentService)
		Dim configEngine As New ConfigEngine.Engine("POSEngine")

		_fulfillmentService = New FulfillmentService(New PlaycardBaseCardManager)
		'_paymentService = New PaymentService
        '   Braian 190603 Inyecto engine
        _paymentService = paymentService
        if(_paymentService Is nothing)
            '   Esto no debería suceder
            _log.Debug("Can't get Payment Service, instancing new engine")
            _paymentService = New PaymentService()
        End If

		_treasuryEnabled = CBool(configEngine.GetItem("Treasury", "Enabled", 0))

		If _treasuryEnabled Then
			_treasuryEngine = New DCS.mPosEngine.Data.Infrastructure.TreasuryEngine
		End If
	End Sub

	Public Function GetTransactionInfo(ByVal transactionDetail As TransactionCartDto, discountsApplied As String, posName As String) As TransactionInfoDto
		Dim retVal As New TransactionInfoDto
		Dim mPosTransaction As MPosTransaction
		Dim transInfo As TransactionTotals
		Dim decimals As Integer

		Dim invoiceNumberGenerator As New InvoiceNumberGenerator(posName)

		_log.Info("Processing GetTransactionInfo")

		_log.Debug("Building transaction from transactionDetail DTO")
		mPosTransaction = (New DtoMapper).GetDomainTransactionFrom(transactionDetail, discountsApplied)

		_log.Debug("Getting transaction totals")
		'Calculate Totals
		'transInfo = calculator.GetTransactionTotals(MPosTransaction)
		transInfo = mPosTransaction.GetTotals
		'****************

		decimals = Core.Configuration.GetInteger("decimalPlaces", 2)

		_log.Debug("Building response DTO")
		'Remap totals to dto********
		retVal.Subtotal1 = Decimal.Round(transInfo.Subtotal1, decimals)
		retVal.Subtotal2 = Decimal.Round(transInfo.Subtotal2, decimals)
		retVal.TotalDiscount = Decimal.Round(transInfo.TotalDiscount, decimals)
		'retVal.TotalTax = Decimal.Round(transInfo.TotalSalesTax, decimals)
		retVal.TotalTax = Decimal.Round(transInfo.TotalTax, decimals)
		retVal.TotalToPay = transInfo.TotalToPay
		retVal.InvoiceNumber = invoiceNumberGenerator.GetNextAndIncrement

		'Tax Info*************        
		Dim itemDto As TaxInfoItemDto
		For Each item As TransactionTaxInfoItem In transInfo.TransactionTaxInfo.TaxItems
			itemDto = New TaxInfoItemDto
			itemDto.TaxName = item.TaxName
			itemDto.TaxAmount = item.TaxAmount
			retVal.TaxDetail.Add(itemDto)
		Next

		Return retVal
	End Function

	Public Function CommitTransactionForTestsOnly(ByVal command As CommitTransactionCommandDto) As CommitTransactionResponseDto
		Dim treasuryWasDisabled As Boolean

		Try
			If Not _treasuryEnabled Then
				treasuryWasDisabled = True
				_treasuryEnabled = True
				_treasuryEngine = New DCS.mPosEngine.Data.Infrastructure.TreasuryEngine
			Else
				treasuryWasDisabled = False
			End If

			Return CommitTransaction(command)

		Finally
			If treasuryWasDisabled Then
				_treasuryEnabled = False
				_treasuryEngine = Nothing
			End If

		End Try

	End Function

	Public Function AuthorizePayment(ByVal payment As IList(Of PayItemDto)) As AuthorizePaymentResponseDto
		Dim paymentData As PaymentData
		Dim dtoMapper As New DtoMapper
		Dim retVal As AuthorizePaymentResponseDto
        Dim transactionId as integer

		_log.Info("Processing AuthorizePayment")

		If payment Is Nothing Then
			Throw New NullCommandDtoException
		End If

		_log.Debug("Extracting payment data")
		paymentData = dtoMapper.GetPaymentData(payment)
        
		_log.Debug("Calling PaymentService.AuthorizePayment")
		Select Case _paymentService.AuthorizePayment(paymentData, transactionId)
			Case PaymentService.PaymentResultEnum.PaymentSuccess
				retVal = New AuthorizePaymentResponseDto(AuthorizePaymentResponseDto.ResultCodesEnum.PaymentAuthorized)
				_log.Info("Payment authorized")
            case PaymentService.PaymentResultEnum.PaymentProcessed
                retVal = New AuthorizePaymentResponseDto(AuthorizePaymentResponseDto.ResultCodesEnum.PaymentAuthorized, transactionId)
				_log.Info("Payment processed")
			Case PaymentService.PaymentResultEnum.NotEnoughPlaycardBalance
				retVal = New AuthorizePaymentResponseDto(AuthorizePaymentResponseDto.ResultCodesEnum.NotEnoughCreditsOnPlaycard)
				_log.Info("Payment not authorized, not enough credits on playcard")
			Case PaymentService.PaymentResultEnum.UnsoldPlaycard
				retVal = New AuthorizePaymentResponseDto(AuthorizePaymentResponseDto.ResultCodesEnum.UnsoldPlaycard)
				_log.Info("Payment not authorized, unsold playcard")
            case else
                retVal = New AuthorizePaymentResponseDto(AuthorizePaymentResponseDto.ResultCodesEnum.UndefinedError)
				_log.Info("Payment not authorized, undefined error")
		End Select

		Return retVal
	End Function

    Public Function CommitTransaction(ByVal command As CommitTransactionCommandDto) As CommitTransactionResponseDto
        Dim df As New NHibernateDaoFactory
        Dim dao As IMPosTransactionDao
        Dim dtoMapper As New DtoMapper
        Dim paymentData As PaymentData
        Dim mPosTransaction As MPosTransaction
        Dim retVal As CommitTransactionResponseDto = Nothing
        Dim success As Boolean

        _log.Info("Processing CommitTransaction")

        dao = df.GetMPosTransactionDao

        If command Is Nothing Then
            Throw New NullCommandDtoException
        End If

        _log.Debug("Validating command data")
        command.Validate()

        _log.Debug("Extracting payment data")
        paymentData = dtoMapper.GetPaymentData(command.Payitems)

        _log.Debug("Extracting transaction data")
        Try
            mPosTransaction = dtoMapper.GetDomainTransactionFrom(command)
            success = True
        Catch ex As ProductNotReturnableException
            _log.Warn("Cannot return product: product [" & ex.ProductName & "] is not returnable")
            retVal = New CommitTransactionResponseDto(CommitTransactionResponseDto.ResultCodesEnum.ProductNotReturnable, ex.ProductName)
            success = False
        Catch ex As CardNotSoldException
            _log.Warn("Cannot sell product: card " & ex.CardNumber & " is not sold")
            retVal = New CommitTransactionResponseDto(CommitTransactionResponseDto.ResultCodesEnum.CardNotSold, ex.CardNumber)
            success = False
        End Try

        Try
            If Not String.IsNullOrWhiteSpace(command.Vouchers) Then
                _log.Debug("Adding vouchers to transaction")
                AddVouchers(paymentData, command.Vouchers, mPosTransaction.GetTotals().TotalToPay)
            End If
        Catch ex As Exception
            _log.Error("Can't add vouchers to transaction. Message: " + ex.Message, ex)
            Return New CommitTransactionResponseDto(CommitTransactionResponseDto.ResultCodesEnum.ErrorAtProcessVoucher, ex.Message)
        End Try

        If success Then
            Using unitOfWork As IUnitOfWork = DCS.ProjectBase.Data.NHibernateSessionMgmt.UnitOfWork.Start
                unitOfWork.Begin()

                '   Braian  190911 Obtengo transacción
                Dim trans = GetTransaction(dao.Session)

                _log.Info("Commiting payment")

                Select Case _paymentService.CommitPayment(command.OperatorId, paymentData)
                    Case PaymentService.PaymentResultEnum.PaymentSuccess

                        If Not String.IsNullOrWhiteSpace(command.Vouchers) Then
                            'If Not RedeemVouchers(command.Vouchers, mPosTransaction.OperatorId, mPosTransaction.PosName, trans) Then
                            '    Return New CommitTransactionResponseDto(CommitTransactionResponseDto.ResultCodesEnum.ErrorAtRedeemVoucher)
                            'End If
                            Try
                                RedeemVouchers(command.Vouchers, mPosTransaction.OperatorId, mPosTransaction.PosName, trans)
                            Catch ex As Exception
                                _log.Error("Error redeeming vouchers", ex)
                                Return New CommitTransactionResponseDto(CommitTransactionResponseDto.ResultCodesEnum.ErrorAtRedeemVoucher, ex.Message)
                            End Try
                        End If


                        mPosTransaction.Pay(paymentData)

                        Try
                            _log.Info("Fulfilling transaction")
                            _fulfillmentService.FulfillTransaction(mPosTransaction)
                            success = True
                        Catch ex As NotEnoughCreditsToReturnException
                            _log.Warn("Fulfillment failed: not enough balance in card to perform the return")
                            success = False
                            retVal = New CommitTransactionResponseDto(CommitTransactionResponseDto.ResultCodesEnum.NotEnoughBalance, ex.CreditTypeName, ex.AmountToReturn, ex.AmountInCard)
                        End Try

                        If success Then
                            _log.Debug("Saving transaction to DB")
                            dao.MakePersistent(mPosTransaction)

                            If _treasuryEnabled Then
                                _log.Info("Treasury is enabled, commiting transaction to treasury")

                                success = (_treasuryEngine.CommitToTreasury(paymentData, mPosTransaction.Id, mPosTransaction.OperatorId, mPosTransaction.PosName) = ITreasuryEngine.CommitResultCodesEnum.CommitSuccess)

                                If Not success Then
                                    _log.Warn("Commit failed: no active treasury session")
                                    retVal = New CommitTransactionResponseDto(CommitTransactionResponseDto.ResultCodesEnum.NoActiveTreasurySession)
                                    success = False
                                End If
                            End If
                        End If

                        If success Then
                            retVal = New CommitTransactionResponseDto(CommitTransactionResponseDto.ResultCodesEnum.CommitSuccess, mPosTransaction.Id)

                            'Talk to external systems**************
                            _log.Info("Searching transaction for external sale operations")
                            For Each operation In mPosTransaction.Operations
                                If operation.GetType = GetType(ProductSellOperation) AndAlso CType(operation, ProductSellOperation).CommitToExternalSystem Then
                                    _log.Info("Commiting to external system for operation [" & operation.ToString & "]")

                                    Dim errorDescription As String

                                    If Not _httpExternalSystemConnector.CommitOperation(operation, errorDescription, mPosTransaction.Id) Then
                                        _log.Error("Commit to external system failed: " & errorDescription)
                                        retVal = New CommitTransactionResponseDto(CommitTransactionResponseDto.ResultCodesEnum.ReportToExternalSystemFailed, errorDescription)
                                        Exit For
                                    End If
                                End If
                            Next
                            '**************************************
                        End If

                    Case PaymentService.PaymentResultEnum.NotEnoughPlaycardBalance
                        _log.Warn("Commit failed: payment failed, not enough balance on paying playcard")
                        retVal = New CommitTransactionResponseDto(CommitTransactionResponseDto.ResultCodesEnum.PaymentFailedNotEnoughPlaycardBalance)
                        success = False
                    Case PaymentService.PaymentResultEnum.UnsoldPlaycard
                        _log.Warn("Commit failed: payment failed, paying playcard not sold")
                        retVal = New CommitTransactionResponseDto(CommitTransactionResponseDto.ResultCodesEnum.PaymentFailedUnsoldPlaycard)
                        success = False
                End Select

                If success Then
                    unitOfWork.Commit()
                    _log.Info("Commit finished successfully")
                Else
                    unitOfWork.Abort()
                End If
            End Using
        End If

        Return retVal
    End Function

    Private Function GetTransaction(session As NHibernate.ISession) As IDbTransaction
        Using command As Common.DbCommand = session.Connection.CreateCommand()
            session.Transaction.Enlist(command)
            Return command.Transaction
        End Using
    End Function


    Public Function GetDiscountsFromProducts(transactionCart As TransactionCartDto) As List(Of DiscountDto)
        Dim res As New List(Of DiscountDto)
        Dim discountEng As New PlaycardBase.Services.DiscountEngine
        Dim discountDao As New PlaycardBase.Data.DiscountDao
        Dim mPosProductDao As ProductDao = (New NHibernateDaoFactory).GetProductDao
        Dim mPosProduct As DCS.mPosEngine.Core.Domain.Sales.Product
        Dim productId As Integer

        For Each lineItem In transactionCart.LineItems
            If lineItem.ExternalSystemData Is Nothing Then
                mPosProduct = mPosProductDao.GetById(lineItem.ProductId)

                If mPosProduct Is Nothing Then
                    _log.Error("mPosProduct id " & lineItem.ProductId & " not found in DB (mPosProducts), throwing an exception")
                    Throw New ApplicationException("mPosProduct id " & lineItem.ProductId & " not found in DB")
                End If

                If mPosProduct.Deleted Or mPosProduct.ProductData.Deleted Then
                    _log.Error("mPosProductId " & mPosProduct.Id & " is deleted")
                    Throw New ApplicationException("mPosProduct id " & mPosProduct.Id & " is deleted")
                End If

                productId = mPosProduct.ProductData.Id
            Else
                _log.Debug("Retrieving CashierTransTypeId " & lineItem.ProductId & " from DB")

                Dim productDao As DCS.PlaycardBase.Core.DataInterfaces.IProductDao = New DCS.PlaycardBase.Data.ProductDao
                Dim productData = productDao.GetById(lineItem.ProductId)

                If productData Is Nothing Then
                    _log.Error("CashierTransTypeId " & lineItem.ProductId & " not found in DB (cnfCashierTransTypes), throwing an exception")
                    Throw New ApplicationException("CashierTransTypeId " & lineItem.ProductId & " not found in DB (cnfCashierTransTypes)")
                End If

                If productData.Deleted Then
                    _log.Error("CashierTransTypeId " & productData.Id & " is deleted")
                    Throw New ApplicationException("CashierTransTypeId " & productData.Id & " is deleted")
                End If

                productId = productData.Id
            End If

            For Each disc In discountDao.FindAll.Where(Function(x) x.Deleted = False)
                Dim discountAmount = discountEng.GetAmountToDiscount(disc.Id, productId, lineItem.UnitPrice, lineItem.Quantity) * lineItem.Quantity

                If discountAmount > 0 Then
                    If res.Any(Function(x) x.Id = disc.Id) Then
                        res.Find(Function(x) x.Id = disc.Id).Amount += discountAmount
                    Else
                        res.Add((New DiscountDto(disc.Id, disc.Name, discountAmount)))
                    End If
                End If
            Next
        Next

        Return res
    End Function

    Private Sub AddVouchers(ByRef paymentData As PaymentData, vouchers As String, totalToPay As Decimal)
        Dim addVouchers As New List(Of String)

        For Each voucherCode In vouchers.Split(",")
            Dim payItemAmount As Decimal

            If addVouchers.Contains(voucherCode) Then
                _log.Error("Repeated voucher " & voucherCode)
                Throw New InvalidOperationException("Repeated voucher " & voucherCode)
            End If

            Dim voucher = CheckVoucher(voucherCode)

            If voucher IsNot Nothing AndAlso voucher.RedeemableForCash AndAlso Not voucher.Redeemed Then
                '   Agrego voucher como si fuese pago en efectivo
                payItemAmount = voucher.Amount

                _log.Debug("Adding to payment data voucher " & voucherCode)

                Dim daoFactory As New DCS.PlaycardBase.Data.NHibernateDaoFactory
                Dim currencyRepo As DCS.PlaycardBase.Core.DataInterfaces.ICurrencyDao = daoFactory.GetCurrencyDao
                Dim currency = currencyRepo.Session.Get(GetType(DCS.PlaycardBase.Core.PosDomain.Currency), 1)

                If currency Is Nothing Then
                    Throw New InvalidDtoFieldException("paymodeId", "Invalid PaymodeId: " & 1)
                End If

                If totalToPay = paymentData.PayItems.Sum(Function(x) x.Amount) Then
                    '   Esto no debería pasar
                    _log.Error("Can't add voucher, total to pay is already paid")
                    Throw New InvalidOperationException("Can't add voucher, total to pay is already paid")
                End If

                '   No puedo pasarme del total de la transacción, sino en treasury van a quedar diferencias
                If (paymentData.PayItems.Sum(Function(x) x.Amount) + voucher.Amount) > totalToPay Then
                    payItemAmount = totalToPay - paymentData.PayItems.Sum(Function(x) x.Amount)
                End If

                addVouchers.Add(voucherCode)

                paymentData.AddPayitem(currency, payItemAmount, PaymodeData.GetPaymodeData(currency.PaymodeType))

                _log.Debug("Amount " & payItemAmount & " added as cash")
            Else
                _log.Error("Voucher " + voucherCode + " can't be added to transaction")
                Throw New InvalidOperationException("Voucher " + voucherCode + " can't be added to transaction")
            End If
        Next

        paymentData.HasVouchers = True
    End Sub

    Public Function CheckVoucher(voucherCode As String) As VoucherResponseDto
        If (voucherCode.Length < 3) Then
            _log.Error("Voucher code cannot be less than 3 caracters")
            Throw New ApplicationException("Voucher code cannot be less than 3 caracters")
        End If

        Dim res As VoucherResponseDto
        Dim voucher As New Voucher()
        Dim voucherDecripted As Long, voucherType As Integer

        voucherType = voucherCode.Substring(0, 2)
        voucherDecripted = DecryptVoucher(voucherCode.Substring(2))

        voucher = GetVoucherById(voucherType, voucherDecripted)

        If voucher IsNot Nothing AndAlso voucher.Id > 0 Then
            res = New VoucherResponseDto(voucher)
        Else
            res = New VoucherResponseDto(-1, False, False)
        End If

        Return res
    End Function

    Private Function DecryptVoucher(voucherCode As String) As String
        Dim voucherDecripted As String
        Dim encrypt As Encrypt.clsEncrypt

        Try
            encrypt = New Encrypt.clsEncrypt()
        Catch ex As Exception
            _log.Error("Can't create encrypt engine. Error: " + ex.Message, ex)
            Throw New ApplicationException("Can't create encrypt engine.")
        End Try

        voucherDecripted = encrypt.DecryptHard(voucherCode, _VoucherHardKey)

        Return voucherDecripted
    End Function

    Private Function GetVoucherById(voucherType As String, voucherId As String) As Voucher
        Dim voucher As Voucher

        Try
            Dim dao As IVoucherDao = (New NHibernateDaoFactory).GetVoucherDao
            voucher = dao.GetVoucherByIdAndType(voucherId, voucherType)
        Catch ex As Exception
            _log.Error("Error at get voucher", ex)
            Return Nothing
        End Try

        Return voucher
    End Function

    Public sub RedeemVouchers(voucherStr As String, operatorId As Integer, posId As String, Optional sqlTrans As SqlClient.SqlTransaction = Nothing) 
        Dim vouchers() = voucherStr.Split(",")

        If vouchers.Count = 0 Then
            throw new ArgumentNullException("voucherStr", "Voucher list is empty")            
        End If

        _log.Info("Redeem vouchers count: " & vouchers.Count)

        For Each voucherEncripted As String In vouchers
            _log.Info("Trying to redeem voucher " + voucherEncripted)

            Dim voucherType As String = voucherEncripted.Substring(0, 2)
            Dim voucherCode As String = DecryptVoucher(voucherEncripted.Substring(2))

            _log.info("Retrieving voucher from DB")
            Dim voucher As Voucher = GetVoucherById(voucherType, voucherCode)

            If voucher Is Nothing OrElse voucher.Id <= 0 Then
                Throw New ApplicationException("Voucher not found in DB (voucherCode=[" & voucherCode & ", voucherType=[" & voucherType & "])")                
            End If

            If Not voucher.RedeemableForCash Then
                Throw New ApplicationException("Voucher is not redeemable for cash (voucherCode=[" & voucherCode & ", voucherType=[" & voucherType & "])")                                
            End If

            If voucher.Redeemed Then
                Throw New ApplicationException("Voucher has already been redeemed (voucherCode=[" & voucherCode & ", voucherType=[" & voucherType & "])")                                                
            End If
            

            dim redeemId=RedeemVoucher(voucherType, voucherCode, voucher.Amount, operatorId)                  
            _log.Info("Voucher redeem success with redeemId: " & redeemId )

            If _treasuryEnabled AndAlso _treasuryEngine IsNot Nothing Then
                _log.Debug("Registering voucher redemption to treasury")
                _treasuryEngine.RegisterVoucher(redeemId, voucherType, voucher.Amount, operatorId, operatorId, posId, sqlTrans)                     
            End If
        Next
    
    End sub

    Private function RedeemVoucher(type As Integer, id As Long, amount As Decimal, cashierId As Integer) as long
        'Dim res As Boolean
        dim voucherRedeemId As Long
        dim dao as ivoucherdao=(new nhibernatedaofactory).getvoucherdao
        
        _log.info("Getting voucher from DB (id=" & id & ", type=" & type & ")")
        Dim voucher=dao.GetVoucherByIdAndType(id,type)

        If voucher Is Nothing then
            Throw New ArgumentOutOfRangeException("Unable to find in DB a voucher with id " & id & " and type " & type )            
        End If

        
        _log.Info("Registering voucher redemption to DB")
        voucherredeemid=dao.RedeemVoucher(voucher,type,cashierid)    
        
        'EXACTAMENTE EN QUE CONDICION DEVUELVE 0???
        If voucherRedeemId = 0 Then     ' Possible error
            Throw New ApplicationException("RedeemId returned 0, unable to register voucher redemption")            
        End If        

        return voucherredeemid
    End function

    Public Function EncryptVoucherOnlyForTest(toEncrypt As String) As String
        Return New Encrypt.clsEncrypt().EncryptHard(toEncrypt, _VoucherHardKey)
    End Function
End Class