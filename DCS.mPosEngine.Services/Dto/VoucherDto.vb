Imports System.Runtime.Serialization
Imports DCS.mPosEngine.Core.Domain.Sales
Imports Newtonsoft.Json

Namespace Dto
    <JsonObject()>
    <DataContract()>
    Public Class VoucherResponseDto
        <DataMember(Name:="amount")>
        <JsonProperty()>
        Public Property Amount As Decimal
        <DataMember(Name:="redeemed")>
        <JsonProperty()>
        Public Property Redeemed As Boolean
        <DataMember(Name:="redeemableForCash")>
        <JsonProperty()>
        Public Property RedeemableForCash As Boolean

        Public Sub New()
        End Sub

        Public Sub New(vAmount As Decimal, vRedeemed As Boolean, vRedeemableForCash As Boolean)
            Redeemed = vRedeemed
            Amount = vAmount
            RedeemableForCash = vRedeemableForCash
        End Sub

        Public Sub New(voucher As Voucher)
            Redeemed = voucher.Redeemed
            Amount = voucher.Amount
            RedeemableForCash = voucher.RedeemableForCash
        End Sub
    End Class
End Namespace