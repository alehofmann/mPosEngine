Imports DCS.mPosEngine.Core.Domain.Shared

Namespace Domain.Sales.Payment
    Public Class PlaycardData
        Inherits paymodeData        
        Implements IPaymodeData


        Private _cardNumber As Long
        Public ReadOnly Property CardNumber() As Long
            Get
                Return _cardNumber
            End Get
        End Property

        Public Sub New(ByVal cardNumber As Long)
            MyBase.New()

            If cardNumber <= 0 Then
                Throw New ArgumentException("cardNumber must be a number greater than 0", cardNumber)
            End If

            _cardNumber = cardNumber
        End Sub
        Public Sub New()

        End Sub

        'Public ReadOnly Property Name As String Implements IPaymodeData.Name
        '    Get
        '        Return "Playcard"
        '    End Get
        'End Property

    End Class
End Namespace
