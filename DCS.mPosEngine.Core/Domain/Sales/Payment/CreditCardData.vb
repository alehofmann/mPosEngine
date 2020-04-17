Imports DCS.mPosEngine.Core.Domain.Shared

Namespace Domain.Sales.Payment
    Public Class CreditCardData
        Inherits PaymodeData        
        Implements IPaymodeData

        Private _cardNumber As String
        Private _cardType As String
        Private _authorizationReference As String
        Private _debugInfo As String
        Public ReadOnly Property DebugInfo() As String
            Get
                Return _debugInfo
            End Get
        End Property

        Public ReadOnly Property AuthorizationReference() As String
            Get
                Return _authorizationReference
            End Get
        End Property

        Public ReadOnly Property CardType() As String
            Get
                Return _cardType
            End Get
        End Property



        Public ReadOnly Property CardNumber() As String
            Get
                Return _cardNumber
            End Get

        End Property

        Public Sub New(ByVal cardNumber As String, ByVal cardType As String, ByVal authorizationReference As String, ByVal debugInfo As String)
            '   Braian 190531 Lo comento por ahora
            'If cardNumber = vbNullString Then
            '    Throw New ArgumentNullException("cardNumber", "Credit Card Number must not be null")
            'End If

            'If cardType = vbNullString Then
            '    Throw New ArgumentNullException("cardType", "Credit Card Type must not be null")
            'End If

            _cardNumber = cardNumber
            _cardType = cardType
            _authorizationReference = authorizationReference
            _debugInfo = debugInfo
        End Sub

        Public Sub New()

        End Sub
    End Class
End Namespace
