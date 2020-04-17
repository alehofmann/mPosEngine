Imports Newtonsoft.Json
Imports System.Runtime.Serialization

Namespace Dto
	<JsonObject()>
	<DataContract()>
	Public Class VoucherItemDto
		<DataMember(Name:="voucherTypeId")>
		<JsonProperty()>
		Public Property VoucherTypeId As Integer
		<DataMember(Name:="voucherAmount")>
		<JsonProperty()>
		Public Property VoucherAmount As Decimal
		<DataMember(Name:="voucherQuantity")>
		<JsonProperty()>
		Public Property VoucherQuantity As Integer

		Public Sub New(voucherType As Integer, amount As Decimal, quantity As Integer)
			Me.VoucherTypeId = voucherType
			Me.VoucherAmount = amount
			Me.VoucherQuantity = quantity
		End Sub
	End Class
End Namespace