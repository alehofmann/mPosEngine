Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto
	<JsonObject()>
	<DataContract()>
	Public Class DiscountDto
		Public Sub New(id As Integer, description As String, amount As Decimal)
			Me.Id = id
			Me.Description = description
			Me.Amount = amount
		End Sub

		<DataMember(Name:="id")>
		<JsonProperty()>
		Public Property Id() As Integer

		<DataMember(Name:="description")>
		<JsonProperty()>
		Public Property Description() As String

		<DataMember(Name:="amount")>
		<JsonProperty()>
		Public Property Amount() As Decimal
	End Class
End Namespace
