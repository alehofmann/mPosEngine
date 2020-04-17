Imports DCS.PlaycardBase.Data
Imports DCS.mPosEngine.Data.Infrastructure
Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment
Imports DCS.mPosEngine.Core.DomainServices
Imports NUnit.Framework
Imports DCS.mPosEngine.Core.Domain.Sales
Imports DCS.mPosEngine.Services
Imports DCS.mPosEngine.Core.Domain.Sales.Payment

Namespace Services

    <TestFixture()> _
    Public Class FulfillmentServiceTests

        <Test()> _
        Public Sub SuccessfullyFulfillTransaction()
            Dim fulfillmentService As New FulfillmentService(New PlaycardBaseCardManager)

            Dim mPosTransaction As MPosTransaction

            mPosTransaction = GetPaidCardRechargeTransaction()
	        
            fulfillmentService.FulfillTransaction(mPosTransaction)

            Assert.IsTrue(mPosTransaction.HasBeenFulfilled)

        End Sub

        <Test()> _
        Public Sub FulfillCreditsReturnOperationNotEnoughCredits()
            Dim fulfillmentService As New FulfillmentService(New PlaycardBaseCardManager)
            Dim mPosTransaction As MPosTransaction

            Dim card As CardInfo = GetSoldCardWithBalance(123111, 10)

            mPosTransaction = GetCreditsReturnTransaction(20, card)
            Try
                fulfillmentService.FulfillTransaction(mPosTransaction)
                Assert.Fail("Expected exception was not thrown")
            Catch ex As NotEnoughCreditsToReturnException

            End Try


        End Sub

        <Test()> _
        Public Sub SuccessfullyFulfillCreditsReturnOperation()
            Dim fulfillmentService As New FulfillmentService(New PlaycardBaseCardManager)
            Dim mPosTransaction As MPosTransaction
            Dim cardManager As New PlaycardBaseCardManager

            Dim card As CardInfo = GetSoldCardWithBalance(123111, 30)

            mPosTransaction = GetCreditsReturnTransaction(20, card)            
            fulfillmentService.FulfillTransaction(mPosTransaction)

            Assert.IsTrue(mPosTransaction.HasBeenFulfilled)
            Assert.IsTrue(cardManager.GetCardCounterAmount(card.CardNumber, 1) = 10)
        End Sub

        Private Function GetSoldCard() As CardInfo
            Dim cardManager As New PlaycardBaseCardManager
            Dim cardNumber As Long = 26955373

            'Set preconditions***************
            If Not cardManager.IsCardSold(cardNumber) Then
                cardManager.SellCard(cardNumber)
            End If
            '********************************

            Return CardManagerFactory.GetCardManager().GetCardInfo(cardNumber)
        End Function

        Private Function GetCardRechargeProduct() As Product
            Dim productDao As DCS.mPosEngine.Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao

            Return productDao.FindByID(2)

        End Function
        Private Function GetPaymentDataForAmount(ByVal amount As Decimal) As PaymentData
            Dim currencyDao As New CurrencyDao

            Return New PaymentData(New PayItem(currencyDao.FindByID(1), amount, New CashPaymodeData()))
        End Function

        Private Function GetPaidCardRechargeTransaction() As MPosTransaction
            Dim retVal As New MPosTransaction(15, "VERUGO","333")
            Dim product As Product


            product = GetCardRechargeProduct()

            retVal.AddLineitem(product.ProductData.Price, 50, product.ProductData, GetSoldCard)
            retVal.Pay(GetPaymentDataForAmount(retVal.GetTotals.TotalToPay))

            Return retVal
        End Function

        Private Function GetCreditsReturnTransaction(ByVal creditsAmount As Decimal, ByVal card As CardInfo) As MPosTransaction
            Dim retVal As New MPosTransaction(15, "VERUGO","333")
            Dim product As Product


            product = GetCardRechargeProduct()

            retVal.AddLineitem(product.ProductData.Price, creditsAmount * -1, product.ProductData, card)
            retVal.Pay(GetPaymentDataForAmount(retVal.GetTotals.TotalToPay))

            Return retVal
        End Function

        Private Function GetSoldCardWithBalance(ByVal cardNumber As Long, ByVal balance As Decimal) As CardInfo
            Dim cardManager As New PlaycardBaseCardManager

            'Set preconditions***************
            If cardManager.IsCardSold(cardNumber) Then
                cardManager.WipeCard(cardNumber)
            End If

            cardManager.SellCard(cardNumber)
            '********************************

            cardManager.ChargeCard(cardNumber, 1, balance)

            Return CardManagerFactory.GetCardManager().GetCardInfo(cardNumber)
        End Function
    End Class

End Namespace
