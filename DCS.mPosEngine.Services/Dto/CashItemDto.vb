Imports Newtonsoft.Json
Imports System.Runtime.Serialization

Namespace Dto
	<JsonObject()>
	<DataContract()>
	Public Class CashItemDto
		<DataMember(Name:="currencyId")>
		<JsonProperty()>
		Public Property CurrencyId As Integer
		<DataMember(Name:="currencyName")>
		<JsonProperty()>
		Public Property CurrencyName As String
		<DataMember(Name:="amount")>
		<JsonProperty()>
		Public Property Amount As Decimal

		Public Sub New(currencyId As Integer, currencyName As String, amount As Decimal)
			Me.CurrencyId = currencyId
			Me.CurrencyName = currencyName
			Me.Amount = amount
		End Sub
	End Class
End Namespace