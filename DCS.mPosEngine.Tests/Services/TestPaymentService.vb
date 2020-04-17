Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment
Imports DCS.mPosEngine.Data.Infrastructure
Imports NUnit.Framework
Imports DCS.mPosEngine.Services.Payment
Imports DCS.mPosEngine.Core.Domain.Sales.Payment

Namespace Services
    <TestFixture()>
    Public Class TestPaymentService
        <Test()>
        Public Sub PlaycardPaymentFailedNotEnoughBalance()
            Dim service As New PaymentService
            Dim payItem As PayItem = New PayItem(GetCurrency(3), 15, New PlaycardData(GetSoldCardWithBalance(10).CardNumber))
            Dim result As Integer

            Assert.AreEqual(service.CommitPayment(12, New PaymentData(payItem)), PaymentService.PaymentResultEnum.NotEnoughPlaycardBalance)
        End Sub

        <Test()>
        Public Sub PlaycardPaymentSuccess()
            Dim service As New PaymentService
            Dim cardManager As New PlaycardBaseCardManager
            Dim card As CardInfo
            Dim payItem As PayItem
            Dim result As Integer

            card = GetSoldCardWithBalance(16)
            payItem = New PayItem(GetCurrency(3), 15, New PlaycardData(card.CardNumber))            
            Assert.AreEqual(service.CommitPayment(12, New PaymentData(payItem)), PaymentService.PaymentResultEnum.PaymentSuccess)

            Assert.IsTrue(cardManager.GetCardCounterAmount(card.CardNumber, 1) = 1)
        End Sub

        <Test()>
        Public Sub PlaycardPaymentFailedUnsoldPlaycard()
            Dim service As New PaymentService
            Dim payItem As PayItem = New PayItem(GetCurrency(3), 15, New PlaycardData(GetNotSoldCard.CardNumber))

            Dim result As Integer            
            Assert.AreEqual(service.CommitPayment(12, New PaymentData(payItem)), PaymentService.PaymentResultEnum.UnsoldPlaycard)
        End Sub

        Private Function GetCurrency(ByVal currencyId As Integer) As DCS.PlaycardBase.Core.PosDomain.Currency
            Dim currency As DCS.PlaycardBase.Core.PosDomain.Currency
            Dim currencyDao As DCS.PlaycardBase.Core.DataInterfaces.ICurrencyDao = (New DCS.PlaycardBase.Data.NHibernateDaoFactory).GetCurrencyDao

            currency = currencyDao.FindById(currencyId)

            Return currency
        End Function

        Private Function GetSoldCardWithBalance(ByVal balance As Decimal) As CardInfo
            Dim cardManager As New PlaycardBaseCardManager
            Dim cardNumber As Long = 26955373
            
            'Set preconditions***************
            If cardManager.IsCardSold(cardNumber) Then
                cardManager.WipeCard(cardNumber)
            End If

            cardManager.SellCard(cardNumber)
            '********************************

            cardManager.ChargeCard(26955373, 1, balance)

            Return CardManagerFactory.GetCardManager().GetCardInfo(cardNumber)
        End Function

        Private Function GetNotSoldCard() As CardInfo
            Dim retVal As CardInfo
            retVal = CardManagerFactory.GetCardManager().GetCardInfo(400000)

            If retVal.IsSold Then
                CardManagerFactory.GetCardManager().WipeCard(400000)
            End If

            retVal = CardManagerFactory.GetCardManager().GetCardInfo(400000)

            Return retVal
        End Function

        <Test()>
        public sub TestGetTransactionStatus()
            Dim service As New PaymentService
            Dim res = service.GetTransactionStatus(324)


        End sub
    End Class
End Namespace
