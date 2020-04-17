Namespace DomainServices
    Public Class NotEnoughCreditsToReturnException
        Inherits ApplicationException

        Private _creditTypeName As String
        Private _amountInCard As Decimal
        Private _amoutToReturn As Decimal

        Public Sub New(ByVal creditTypeName As String, ByVal amountInCard As Decimal, ByVal amountToReturn As Decimal)
            MyBase.New("Cannot Return " & amountToReturn & " " & creditTypeName & ". Only " & amountInCard & " in card")

            _creditTypeName = creditTypeName
            _amountInCard = amountInCard
            _amoutToReturn = amountToReturn
        End Sub

        Public ReadOnly Property CreditTypeName As String
            Get
                Return _creditTypeName
            End Get
        End Property

        Public ReadOnly Property AmountInCard As Decimal
            Get
                Return _amountInCard
            End Get
        End Property

        Public ReadOnly Property AmountToReturn As Decimal
            Get
                Return _amoutToReturn
            End Get
        End Property

    End Class
End Namespace
