Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment

Namespace Infrastructure
    Public Class MockCardManager
        Implements ICardManager












        Public Function IsCardSold(cardNumber As Long) As Boolean Implements ICardManager.IsCardSold
            Return (cardNumber = 300000)
        End Function

        Public Sub ChangeCardStatus(cardNumber As Long, newStatusId As Integer) Implements ICardManager.ChangeCardStatus

        End Sub

        Public Sub ChargeCard(cardNumber As Long, counterTypeId As Integer, amountToCharge As Decimal) Implements ICardManager.ChargeCard

        End Sub

        Public Sub SellCard(cardNumber As Long) Implements ICardManager.SellCard

        End Sub

        Public Sub WipeCard(cardNumber As Long) Implements ICardManager.WipeCard

        End Sub

        Public Function GetCardCounterAmount(cardNumber As Long, counterTypeId As Integer) As Decimal Implements ICardManager.GetCardCounterAmount

        End Function

        Public Function GetCardInfo(cardNumber As Long) As Core.Domain.Sales.Fulfillment.CardInfo Implements ICardManager.GetCardInfo
            If cardNumber = 300000 Then
                Return New CardInfo(300000, True)
            Else
                Return New CardInfo(cardNumber, False)
            End If
        End Function

        Public Sub DebitFromCard(cardNumber As Long, counterTypeId As Integer, amountToDebit As Decimal) Implements Core.DataInterfaces.ICardManager.DebitFromCard

        End Sub


        
        Public Function TransferCard(sourceCardNumber As Long, destCardNumber As Long, posId As String, operatorId As Integer) As Core.DataInterfaces.ICardManager.CardTransferResultCodesEnum Implements Core.DataInterfaces.ICardManager.TransferCard

        End Function

        Public Function ConsolidateCards(sourceCardNumbers As Microsoft.VisualBasic.Collection, destCardNumber As Long, posId As String, operatorId As Integer, Optional ByRef failedCard As Long = 0) As Core.DataInterfaces.ICardManager.CardTransferResultCodesEnum Implements Core.DataInterfaces.ICardManager.ConsolidateCards

        End Function
    End Class
End Namespace
