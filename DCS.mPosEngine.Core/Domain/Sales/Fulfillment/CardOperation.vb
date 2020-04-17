Imports DCS.PlaycardBase.CardData
Imports DCS.PlaycardBase.Core.CardDomain

Namespace Domain.Sales.Fulfillment
    Public Class CardOperation
        Private _counterTransaction As CounterTransaction
        Private _operationType As CardOperationTypesEnum
        Private _card As CardInfo
        Private _newStatusId As Integer?        

        Public Function MovesCounters() As Boolean
            Return (_counterTransaction IsNot Nothing AndAlso _counterTransaction.Items.Count > 0)
        End Function

        Public Property OperationType() As CardOperationTypesEnum
            Get
                Return _operationType
            End Get
            Private Set(value As CardOperationTypesEnum)
                _operationType = value
            End Set
        End Property

        Public Enum CardOperationTypesEnum
            CardSell = 1
            CardRecharge = 2
            CardUncharge = 3
            CardReturn = 4
        End Enum

        Public Function ChangesCardStatus() As Boolean
            Return (NewStatusId.HasValue)
        End Function
        Public Property NewStatusId As Integer?
            Get
                Return _newStatusId
            End Get
            Private Set(value As Integer?)
                _newStatusId = value
            End Set
        End Property

        Public Sub New(ByVal cardInfo As CardInfo, ByVal opType As CardOperationTypesEnum, ByVal newStatusId As Integer?, Optional ByVal counterTransaction As CounterTransaction = Nothing)

            If cardInfo Is Nothing Then Throw New ArgumentNullException("cardInfo")

            If opType = CardOperationTypesEnum.CardSell And cardInfo.IsSold Then
                Throw New CardAlreadySoldException(cardInfo.CardNumber)
            End If

            If opType = CardOperationTypesEnum.CardReturn And Not cardInfo.IsSold Then
                Throw New CardNotSoldException(cardInfo.CardNumber)
            End If


            _card = cardInfo
            _operationType = opType
            _counterTransaction = counterTransaction
            _newStatusId = newStatusId
        End Sub


        Public Property SellsNewCard As Boolean
            Get
                Return _operationType = CardOperationTypesEnum.CardSell
            End Get
            Private Set(value As Boolean)
                _operationType = value
            End Set
        End Property

        Public Property CounterTransaction() As CounterTransaction
            Get
                Return _counterTransaction
            End Get
            Set(ByVal value As CounterTransaction)
                _counterTransaction = value
            End Set
        End Property

        Public ReadOnly Property Card As CardInfo
            Get
                Return _card
            End Get
        End Property

        Private Sub New()

        End Sub
    End Class
End Namespace
