Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto
	<JsonObject()>
	<DataContract()>
	Public Class MoneyOperationDto
		<DataMember(Name:="currencyId")>
		<JsonProperty()>
		Public Property CurrencyId As Integer

		<DataMember(Name:="amount")>
		<JsonProperty()>
		Public Property Amount As Decimal
	End Class
End Namespace