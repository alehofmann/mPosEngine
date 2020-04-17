

Namespace Domain.Sales.Fulfillment

    Public Class OperationFulfillmentData
        Private _cardOperation As CardOperation

        Public Function HasCardOperation() As Boolean
            Return _cardOperation IsNot Nothing
        End Function

        Public Property CardOperation() As CardOperation
            Get
                Return _cardOperation
            End Get
            Set(ByVal value As CardOperation)
                _cardOperation = value
            End Set
        End Property

        'Public Sub AddCardOperation(ByVal operation As CardOperation)
        '    If operation Is Nothing Then Throw New ArgumentNullException("operation")

        '    If operation.OperationType = CardOperation.CardOperationTypesEnum.CardRecharge And Not operation.CardInfo.IsSold And Not HasCardSellOperation() Then
        '        Throw New CardNotSoldException(operation.CardInfo.CardNumber)
        '    End If

        '    _cardOperations.Add(operation)

        'End Sub

        'Private Function HasCardSellOperation() As Boolean
        '    If _cardOperations Is Nothing Then
        '        Return False
        '    End If

        '    For Each operation As CardOperation In _cardOperations
        '        If operation.OperationType = CardOperation.CardOperationTypesEnum.CardSell Then
        '            Return True
        '        End If
        '    Next

        '    Return False
        'End Function
    End Class

End Namespace
