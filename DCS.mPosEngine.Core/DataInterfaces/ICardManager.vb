Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment

Namespace DataInterfaces
    Public Interface ICardManager
        Enum CardTransferResultCodesEnum
            TransferSuccess = 0
            InvalidSourceCard = 1
            InvalidDestinationCard = 2
        End Enum


        Sub ChargeCard(ByVal cardNumber As Long, ByVal counterTypeId As Integer, ByVal amountToCharge As Decimal)
        Function IsCardSold(ByVal cardNumber As Long) As Boolean
        Sub ChangeCardStatus(ByVal cardNumber As Long, ByVal newStatusId As Integer)
        Sub SellCard(ByVal cardNumber As Long)
        Sub WipeCard(ByVal cardNumber As Long)
        Function GetCardCounterAmount(ByVal cardNumber As Long, ByVal counterTypeId As Integer) As Decimal
        Function GetCardInfo(ByVal cardNumber As Long) As CardInfo
        Sub DebitFromCard(ByVal cardNumber As Long, ByVal counterTypeId As Integer, ByVal amountToDebit As Decimal)
        Function TransferCard(ByVal sourceCardNumber As Long, ByVal destCardNumber As Long, ByVal posId As String, ByVal operatorId As Integer) As CardTransferResultCodesEnum
        Function ConsolidateCards(ByVal sourceCardNumbers As Collection, ByVal destCardNumber As Long, ByVal posId As String, ByVal operatorId As Integer, Optional ByRef failedCard As Long = 0) As CardTransferResultCodesEnum

    End Interface
End Namespace
