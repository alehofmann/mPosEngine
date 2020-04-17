Namespace Domain.Sales
    Public Class Voucher
        Public Property Id As Integer
        Public Property Amount As Decimal
        Public Property Redeemed As Boolean
        Public Property RedeemableForCash As Boolean

        Public Overrides Function GetHashCode() As Integer
            Return (GetType(Voucher).FullName).GetHashCode
        End Function
    End Class
End Namespace