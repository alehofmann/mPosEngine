Imports DCS.mPosEngine.Core.DataInterfaces

Namespace DomainServices
    Class InvalidSourceCardForTransferException
        Inherits ApplicationException
    End Class

    Class InvalidDestinationCardForTransferException
        Inherits ApplicationException
    End Class


    Public Class CardTransferService
        Private _cardManager As ICardManager

        Public Sub New(ByVal cardManager As ICardManager)
            _cardManager = cardManager
        End Sub

        Public Sub CardTransfer(ByVal sourceCardNumber As Long, ByVal destCardNumber As Long)
            If sourceCardNumber = 0 Then
                Throw New ArgumentNullException("sourceCardNumber")
            End If

            If sourceCardNumber = 0 Then
                Throw New ArgumentNullException("destCardNumber")
            End If

            If Not _cardManager.IsCardSold(sourceCardNumber) Then
                Throw New InvalidSourceCardForTransferException
            End If

            If _cardManager.IsCardSold(destCardNumber) Then
                Throw New InvalidDestinationCardForTransferException
            End If

        End Sub


    End Class
End Namespace

