Namespace Domain.Sales
    Public Class RedeemVoucher
        Public Property Type As Integer
        Public Property Id As Long
        Public Property Amount As Decimal
        Public Property CashierId As Integer
        Public Property RedeemId As Long

        Public Overrides Function GetHashCode() As Integer
            Return (GetType(RedeemVoucher).FullName).GetHashCode
        End Function
    End Class
End Namespace