Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment
Imports DCS.mPosEngine.Data.Infrastructure
Imports NUnit.Framework

Namespace Infrastructure

    <TestFixture()> _
    Public Class TestCardManager
        Dim _cardManager As New Data.Infrastructure.PlaycardBaseCardManager

        <Test()>
        Public Sub WipeCard()
            Dim testCardNumber As Long = 400000

            'Preconditions Check
            _cardManager.SellCard(testCardNumber)
            Assert.IsTrue(_cardManager.IsCardSold(testCardNumber))
            '********************

            _cardManager.WipeCard(testCardNumber)
            Assert.IsFalse(_cardManager.IsCardSold(testCardNumber))
        End Sub

        <Test()>
        Public Sub SellCard()
            Dim testCardNumber As Long = 400000

            'Preconditions Check
            _cardManager.WipeCard(testCardNumber)
            Assert.IsFalse(_cardManager.IsCardSold(testCardNumber))
            '*******************

            _cardManager.SellCard(testCardNumber)
            Assert.IsTrue(_cardManager.IsCardSold(testCardNumber))

        End Sub

        <Test()>
        Public Sub IsCardSold()
            Dim testCardNumber As Long = 400000

            _cardManager.WipeCard(testCardNumber)
            Assert.IsFalse(_cardManager.IsCardSold(testCardNumber))
        End Sub

        <Test()>
        Public Sub ChargeCredits()
            Dim testCardNumber As Long = 12435789
            'Dim counterTypeDao As DCS.PlaycardBase.Core.DataInterfaces.ICounterTypeDao = New DCS.PlaycardBase.Data.CounterTypeDao
            'Dim creditsCounterType = counterTypeDao.FindById(1)
            Dim originalBalance As Decimal
            Dim newBalance As Decimal
            Dim startingChargeAmount As Decimal = 20

            'Preconditions*************
            If Not _cardManager.IsCardSold(testCardNumber) Then
                _cardManager.WipeCard(testCardNumber)
                _cardManager.SellCard(testCardNumber)
            End If
            '**************************

            originalBalance = _cardManager.GetCardCounterAmount(testCardNumber, 1)

            _cardManager.ChargeCard(testCardNumber, 1, 1)

            newBalance = _cardManager.GetCardCounterAmount(testCardNumber, 1)

            Assert.That(newBalance = originalBalance + 4)
        End Sub

        <Test()>
        Public Sub TestSuccessfulCardTransfer()
            Dim sourceCard As CardInfo
            Dim destCard As CardInfo
            Dim cardManager As New PlaycardBaseCardManager

            Dim expectedCredits As Decimal
            Dim expectedBonus As Decimal
            Dim expectedCourtesy As Decimal
            Dim expectedTickets As Decimal            

            sourceCard = GetSoldCard()
            destCard = GetNotSoldCard()

            cardManager.ChargeCard(sourceCard.CardNumber, 1, 10)
            cardManager.ChargeCard(sourceCard.CardNumber, 2, 20)
            cardManager.ChargeCard(sourceCard.CardNumber, 3, 30)
            cardManager.ChargeCard(sourceCard.CardNumber, 5, 40)

            expectedCredits = cardManager.GetCardCounterAmount(sourceCard.CardNumber, 1)
            expectedBonus = cardManager.GetCardCounterAmount(sourceCard.CardNumber, 2)
            expectedCourtesy = cardManager.GetCardCounterAmount(sourceCard.CardNumber, 3)
            expectedTickets = cardManager.GetCardCounterAmount(sourceCard.CardNumber, 5)

            Assert.AreEqual(ICardManager.CardTransferResultCodesEnum.TransferSuccess, cardManager.TransferCard(sourceCard.CardNumber, destCard.CardNumber, "VERUGO", 15))


            Assert.IsFalse(cardManager.IsCardSold(sourceCard.CardNumber))
            Assert.IsTrue(cardManager.IsCardSold(destCard.CardNumber))

            Assert.AreEqual(expectedCredits, cardManager.GetCardCounterAmount(destCard.CardNumber, 1))
            Assert.AreEqual(expectedBonus, cardManager.GetCardCounterAmount(destCard.CardNumber, 2))
            Assert.AreEqual(expectedCourtesy, cardManager.GetCardCounterAmount(destCard.CardNumber, 3))
            Assert.AreEqual(expectedTickets, cardManager.GetCardCounterAmount(destCard.CardNumber, 5))

        End Sub

        <Test()> _
        Public Sub TestFailedCardTransferInvalidSourceCard()
            Dim sourceCard As CardInfo
            Dim destCard As CardInfo
            Dim cardManager As New PlaycardBaseCardManager

            sourceCard = GetNotSoldCard(400000)
            destCard = GetNotSoldCard(400001)

            Assert.AreEqual(ICardManager.CardTransferResultCodesEnum.InvalidSourceCard, cardManager.TransferCard(sourceCard.CardNumber, destCard.CardNumber, "VERUGO", 15))
            

        End Sub

        <Test()> _
        Public Sub TestFailedCardConsolidateInvalidSourceCard()
            Dim sourceCards As New Collection
            Dim sourceCard1 As Long
            Dim sourceCard2 As Long
            Dim result As ICardManager.CardTransferResultCodesEnum
            Dim failedCard As Long

            Dim destCard As Long
            Dim cardManager As New PlaycardBaseCardManager

            sourceCard1 = GetSoldCard(500000).CardNumber
            sourceCard2 = GetNotSoldCard(500001).CardNumber
            destCard = GetSoldCard(500002).CardNumber

            sourceCards.Add(sourceCard1)
            sourceCards.Add(sourceCard2)

            result = cardManager.ConsolidateCards(sourceCards, destCard, "VERUGO", 15, failedCard)

            Assert.AreEqual(ICardManager.CardTransferResultCodesEnum.InvalidSourceCard, result)
            Assert.AreEqual(500001, failedCard)
        End Sub
        <Test()> _
        Public Sub TestFailedCardTransferInvalidDestinationCard()
            Dim sourceCard As CardInfo
            Dim destCard As CardInfo
            Dim cardManager As New PlaycardBaseCardManager

            sourceCard = GetNotSoldCard(400000)
            destCard = GetSoldCard()

            Assert.AreEqual(ICardManager.CardTransferResultCodesEnum.InvalidDestinationCard, cardManager.TransferCard(sourceCard.CardNumber, destCard.CardNumber, "VERUGO", 15))


        End Sub

        Private Function GetSoldCard(Optional ByVal cardNumber As Long = 26955373) As CardInfo
            Dim cardManager As New PlaycardBaseCardManager            

            'Set preconditions***************
            If Not cardManager.IsCardSold(cardNumber) Then
                cardManager.SellCard(cardNumber)
            End If
            '********************************

            Return CardManagerFactory.GetCardManager().GetCardInfo(cardNumber)
        End Function
        Private Function GetNotSoldCard(Optional ByVal cardNumber As Long = 40000) As CardInfo
            Dim retVal As CardInfo
            retVal = CardManagerFactory.GetCardManager().GetCardInfo(cardNumber)

            If retVal.IsSold Then
                CardManagerFactory.GetCardManager().WipeCard(cardNumber)
            End If

            retVal = CardManagerFactory.GetCardManager().GetCardInfo(cardNumber)

            Return retVal
        End Function

        <Test()> _
        Public Sub TestChangeCardStatus()
            CardManagerFactory.GetCardManager.ChangeCardStatus(6621952, 1)
        End Sub
    End Class

End Namespace
