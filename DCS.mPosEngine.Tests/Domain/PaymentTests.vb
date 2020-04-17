Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment
Imports DCS.mPosEngine.Data.Infrastructure
Imports NUnit.Framework
Imports DCS.mPosEngine.Core.Domain.Sales
Imports DCS.mPosEngine.Core.Domain.Sales.Payment

Namespace Domain
    <TestFixture()>
    Public Class PaymentTests
        <Test()> _
        Public Sub CannotPayInvalidPaymentAmount()
            Dim mPosTransaction As MPosTransaction
            Dim currency As DCS.PlaycardBase.Core.PosDomain.Currency

            currency = GetCurrency(1)
            mPosTransaction = GetCardRechargeTransaction()

            Try
                '_purchasingServices.CommitTransaction(command)
                mPosTransaction.Pay(New PaymentData(New PayItem(currency, mPosTransaction.GetTotals.TotalToPay - 1, New CashPaymodeData())))
                Assert.Fail("Expecting Exception and didn't happened")
            Catch ex As DCS.mPosEngine.Core.Domain.Sales.InvalidPaymentAmountException

            End Try

        End Sub

        Private Function GetCurrency(ByVal currencyId As Integer) As DCS.PlaycardBase.Core.PosDomain.Currency
            Dim currency As DCS.PlaycardBase.Core.PosDomain.Currency
            Dim currencyDao As DCS.PlaycardBase.Core.DataInterfaces.ICurrencyDao = (New DCS.PlaycardBase.Data.NHibernateDaoFactory).GetCurrencyDao

            currency = currencyDao.FindById(currencyId)

            Return currency
        End Function

        <Test()> _
        Public Sub TransactionIsNotFullyPaid()
            Dim mPosTransaction As MPosTransaction
            mPosTransaction = GetCardRechargeTransaction()

            Assert.IsFalse(mPosTransaction.IsFullyPaid)
        End Sub

        <Test()> _
        Public Sub TransactionIsFullyPaidTest()
            Dim mPosTransaction As MPosTransaction
            Dim currency As DCS.PlaycardBase.Core.PosDomain.Currency

            currency = GetCurrency(1)
            mPosTransaction = GetCardRechargeTransaction()

            mPosTransaction.Pay(New PaymentData(New PayItem(currency, mPosTransaction.GetTotals.TotalToPay, New CashPaymodeData())))

            Assert.IsTrue(mPosTransaction.IsFullyPaid)
        End Sub

        <Test()> _
        Public Sub TransactionIsFullyPaidMultipleCashTest()
            Dim mPosTransaction As MPosTransaction
            Dim currency As DCS.PlaycardBase.Core.PosDomain.Currency
            Dim paymentData As New PaymentData

            currency = GetCurrency(1)
            mPosTransaction = GetCardRechargeTransaction()

            paymentData.AddPayitem(currency, mPosTransaction.GetTotals.TotalToPay / 2, New CashPaymodeData())
            paymentData.AddPayitem(currency, mPosTransaction.GetTotals.TotalToPay / 2, New CashPaymodeData())

            mPosTransaction.Pay(paymentData)

            Assert.IsTrue(mPosTransaction.IsFullyPaid)
        End Sub

        Private Function GetCardRechargeTransaction() As MPosTransaction
			Dim retVal As New MPosTransaction(15, "VERUGO", 1)
			Dim productDao As DCS.mPosEngine.Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao
            Dim product As Product

            product = productDao.FindByID(71)


            retVal.AddLineitem(product.ProductData.Price, 50, product.ProductData, GetSoldCard)


            Return retVal
        End Function

        
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
    End Class
End Namespace