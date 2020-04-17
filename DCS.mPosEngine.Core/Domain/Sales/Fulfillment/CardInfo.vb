Imports DCS.mPosEngine.Core.Domain.Shared

Namespace Domain.Sales.Fulfillment
    Public Class CardInfo
        Implements IValueObject(Of CardInfo)
        Private _cardNumber As Long
        Private _isSold As Boolean

        Private Sub New()

        End Sub

        Public Sub New(ByVal cardNumber As Long, ByVal isSold As Boolean)
            _cardNumber = cardNumber
            _isSold = isSold
        End Sub

        Public ReadOnly Property IsSold As Boolean
            Get
                Return _isSold
            End Get
        End Property

        Public Property CardNumber As Long
            Get
                Return _cardNumber
            End Get
            Private Set(value As Long)
                _cardNumber = value
            End Set
        End Property

        Public Function SameValueAs(candidate As CardInfo) As Boolean Implements [Shared].IValueObject(Of CardInfo).SameValueAs

        End Function
    End Class
End Namespace