
Imports DCS.mPosEngine.Core.Domain.Sales
Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment
Imports DCS.mPosEngine.Core.Domain.Sales.Payment
Imports DCS.mPosEngine.Data.Infrastructure
Imports DCS.mPosEngine.Services
Imports DCS.mPosEngine.Services.Dto
Imports DCS.PlaycardBase.Core.PosDomain

Public Class Helpers
	Private Shared _operatorId As Integer = 1
	Private Shared _posId As String = "VERUGO"
	Private shared _purchasingServices As New PurchasingServices
	Public Shared Function GetSingleLineTransaction(productId As Integer, settledPrice As Decimal, quantity As Decimal, Optional cardNumber As Integer = 0)
		Dim transaction As New MPosTransaction(_operatorId, _posId)
		'Dim productDao As Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao
		Dim productDao As PlaycardBase.Core.DataInterfaces.IProductDao = (New PlaycardBase.Data.NHibernateDaoFactory).GetProductDao
		'Dim product As Core.Domain.Sales.Product
		Dim product As PlaycardBase.Core.PosDomain.Product

		Dim cardInfo As CardInfo = Nothing
		Dim paymentData As PaymentData


		product = productDao.FindById(productId)

		If cardNumber <> 0 Then
			cardInfo = CardManagerFactory.GetCardManager().GetCardInfo(cardNumber)
		End If

		transaction.AddLineitem(settledPrice, quantity, product, cardInfo)

		paymentData = GetCashPaymentData(transaction.GetTotals.TotalToPay)
		transaction.Pay(paymentData)

		Return transaction
	End Function

	Private shared sub WipeSellAndChargeCard(cardNumber As Long, creditsAmount As Decimal)
		Dim cardManager As New Data.Infrastructure.PlaycardBaseCardManager

		cardManager.WipeCard(cardNumber)
		cardManager.SellCard(cardNumber)
		cardManager.ChargeCard(cardNumber, 1, creditsAmount)		
	End Sub
	Public shared Function GetCommitTransactionCommand(cardNumber As Long, productId As Integer, quantity As Decimal, unitPrice As Decimal, paymodeId As Integer) As CommitTransactionCommandDto 
		Dim cart As New TransactionCartDto
		Dim li As LineitemDto
		Dim ti As TransactionInfoDto
		Dim command As New CommitTransactionCommandDto
		Dim response As CommitTransactionResponseDto

		
		wipeSellAndChargeCard(cardNumber, 0)

		li = New LineitemDto
		li.CardNumber = cardNumber
		li.ProductId = productId 
		li.Quantity = quantity 
		li.UnitPrice = unitPrice 

		cart.LineItems.Add(li)

		ti = _purchasingServices.GetTransactionInfo(cart)

		command.Cart = cart
		command.Payitems.Add(New PayItemDto(paymodeId, ti.TotalToPay, 0, "", "", "", ""))
		command.PosName = _posid
		command.InvoiceNumber = ti.InvoiceNumber
		command.OperatorId = _operatorId 

		Return command 
	End Function
	Public Shared Function GetCashPaymentData(amount As Decimal) As PaymentData
		Dim currencyDao As PlaycardBase.Core.DataInterfaces.ICurrencyDao
		Dim currency As Currency

		currencyDao = (New PlaycardBase.Data.NHibernateDaoFactory).GetCurrencyDao
		currency = currencyDao.FindById(1)

		Return New Payment.PaymentData(New PayItem(currency, amount, New CashPaymodeData))
	End Function
End Class
