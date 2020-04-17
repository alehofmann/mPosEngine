Imports DCS.mPosEngine.Core.Domain.Shared

Namespace Domain.Sales.Payment
    Public Class CashPaymodeData
        Inherits paymodeData
        Implements IpaymodeData



        'Private _currencyId As Integer
        'Public ReadOnly Property CurrencyId() As Integer
        '    Get
        '        Return _currencyId
        '    End Get
        'End Property

        Public Sub New()
            'MyBase.New(1, "Cash")
            '_currencyId = currencyId
        End Sub



    End Class
End Namespace
