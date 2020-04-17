Imports Newtonsoft.Json
Imports System.Runtime.Serialization

Namespace Dto
	<JsonObject()>
	<DataContract()>
	Public Class TreasuryBalanceDto
		<DataMember(Name:="counters")>
		<JsonProperty()>
		Public Property Cash As List(Of CashItemDto)
		<DataMember(Name:="vouchers")>
		<JsonProperty()>
		Public Property Vouchers As List(Of VoucherItemDto)

		Public Sub New()
			Cash = New List(Of CashItemDto)
			Vouchers = New List(Of VoucherItemDto)
		End Sub

		Public Sub AddCash(currencyId As Integer, currencyName As String, amount As Decimal)
			If Cash.Exists(Function(x) x.CurrencyId = currencyId) Then
				Cash.First(Function(x) x.CurrencyId = currencyId).Amount += amount
			Else
				Cash.Add(New CashItemDto(currencyId, currencyName, amount))
			End If
		End Sub

		Public Sub AddVoucher(voucherTypeId As Integer, voucherAmount As Decimal, voucherCount As Integer)
			If Vouchers.Exists(Function(x) x.VoucherTypeId = voucherTypeId And x.VoucherAmount = voucherAmount) Then
				Vouchers.First(Function(x) x.VoucherTypeId = voucherTypeId And x.VoucherAmount = voucherAmount).VoucherQuantity += voucherCount
			Else
				Vouchers.Add(New VoucherItemDto(voucherTypeId, voucherAmount, voucherCount))
			End If
		End Sub
	End Class
End Namespace