Imports DCS.PlaycardBase.Core.PosDomain

Namespace Domain.Sales.Payment
    Public Class PayItem
        Private _currency As Currency
        Private _amount As Decimal, _gratuity As decimal
        Private _paymodeData As IPaymodeData

        Public Sub New(ByVal currency As Currency, ByVal amount As Decimal, ByVal paymodeData As IPaymodeData, optional gratuity As Decimal = 0)
            _currency = currency
            _amount = amount
            _paymodeData = paymodeData
            _gratuity = gratuity
        End Sub

        Private Sub New()
        End Sub

        Sub New(currency As PlaycardBase.Core.PosDomain.Currency)
            ' TODO: Complete member initialization 
            _currency = currency
        End Sub

        Public ReadOnly Property PaymodeData() As IPaymodeData
            Get
                Return _paymodeData
            End Get
        End Property

        Public ReadOnly Property Amount() As Decimal
            Get
                Return _amount
            End Get
        End Property

        Public readonly Property Gratuity() As decimal
            Get 
                return _gratuity
            End Get
        End Property

        Public ReadOnly Property Currency() As Currency
            Get
                Return _currency
            End Get
        End Property
    End Class
End Namespace