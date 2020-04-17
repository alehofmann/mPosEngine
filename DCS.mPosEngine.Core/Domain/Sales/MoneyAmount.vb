'Namespace Domain.Sales
'    Public Class MoneyAmount
'        Private ReadOnly _sourceAmount As Decimal

'        Public Sub New(ByVal amount As Decimal)
'            _sourceAmount = amount
'        End Sub

'        Public Function Add(ByVal amount As Decimal) As Decimal
'            Return New Decimal(_sourceAmount + amount)
'        End Function

'        Public Function Add(ByVal amount As Decimal) As Decimal
'            Return New Decimal(_sourceAmount + amount.value)
'        End Function

'        Public ReadOnly Property Value As Decimal
'            Get
'                Return Decimal.Round(_sourceAmount, Configuration.GetInteger("decimalPlaces", 2))
'            End Get
'        End Property
'    End Class
'End Namespace
