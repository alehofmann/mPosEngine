Imports DCS.PlaycardBase.Core.PosDomain

Namespace Domain.Sales.Payment
    Public Class PaymentData
        Private _payitems As IList(Of PayItem) = New List(Of PayItem)
        'Private _parentTransaction As MPosTransaction

        'Friend Property ParentTransaction() As MPosTransaction
        '    Get
        '        Return _parentTransaction
        '    End Get
        '    Set(ByVal value As MPosTransaction)
        '        For Each item As PayItem In _payitems
        '            item.ParentTransaction = value
        '        Next                
        '    End Set
        'End Property

        
        Public Sub New()

        End Sub
        Public Sub New(ByVal firstPayitem As PayItem)
            _payitems.Add(firstpayitem)
        End Sub

        Public Property PayItems() As IList(Of PayItem)
            Get
                Return _payitems
            End Get
            Set(ByVal value As IList(Of PayItem))
                _payitems = value
            End Set
        End Property

        Public Sub AddPayitem(ByVal currency As Currency, ByVal amount As Decimal, ByVal creditCardNumber As String, ByVal creditCardType As String, ByVal authorizerReference As String, ByVal debugInfo As String)
            If currency.PaymodeType <> PlaycardBase.Core.PosDomain.Currency.PaymodeTypesEnum.CreditCardPaymodeType Then
                Throw New ApplicationException("Paymode Type must be 'Credit Card'")
            End If
            If creditCardNumber = "" Then
                Throw New ArgumentException("creditCardNumber must not be null", "creditCardNumber")
            End If


        End Sub

        Public Sub AddPayitem(ByVal currency As Currency, ByVal amount As Decimal, ByVal paymodeData As IPaymodeData)
            'If currency.PaymodeType = PlaycardBase.Core.PosDomain.Currency.PaymodeTypesEnum.CreditCardPaymodeType And paymodeData.GetType IsNot GetType(CreditCardData) Then
            ' Throw New ArgumentException("Must provide CreditCardData for paymode type " & currency.Id)
            'ElseIf currency.PaymodeType = PlaycardBase.Core.PosDomain.Currency.PaymodeTypesEnum.PlaycardPaymodeType And paymodeData.GetType IsNot GetType(PlaycardData) Then
            'Throw New ArgumentException("Must provide PlaycardData for paymode type " & currency.Id)
            'End If

            _payitems.Add(New PayItem(currency, amount, paymodeData))
        End Sub

        Public Sub AddPayitem(ByVal currency As Currency, ByVal amount As Decimal, gratuity As Decimal, ByVal paymodeData As IPaymodeData)
            'If currency.PaymodeType = PlaycardBase.Core.PosDomain.Currency.PaymodeTypesEnum.CreditCardPaymodeType And paymodeData.GetType IsNot GetType(CreditCardData) Then
            ' Throw New ArgumentException("Must provide CreditCardData for paymode type " & currency.Id)
            'ElseIf currency.PaymodeType = PlaycardBase.Core.PosDomain.Currency.PaymodeTypesEnum.PlaycardPaymodeType And paymodeData.GetType IsNot GetType(PlaycardData) Then
            'Throw New ArgumentException("Must provide PlaycardData for paymode type " & currency.Id)
            'End If

            _payitems.Add(New PayItem(currency, amount, paymodeData, gratuity))
        End Sub

        Public Function GetTotalAmount()
            Dim retVal As Decimal = 0

            For Each payItem As PayItem In _payitems
                retVal += payItem.Amount
            Next

            Return Decimal.Round(retVal, Configuration.GetInteger("decimalPlaces", 2))
        End Function

        Public Property HasVouchers() As Boolean
    End Class
End Namespace
