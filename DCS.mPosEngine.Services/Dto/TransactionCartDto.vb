Imports Newtonsoft.Json
Imports System.Runtime.Serialization

Namespace Dto
	<JsonObject()>
	<DataContract()>
	Public Class TransactionCartDto

		Private _lineItems As IList(Of LineitemDto) = New List(Of LineitemDto)

		<DataMember(Name:="lineItems")>
		<JsonProperty()>
		Public Property LineItems() As IList(Of LineitemDto)
			Get
				Return _lineItems
			End Get
			Set(ByVal value As IList(Of LineitemDto))
				_lineItems = value
			End Set
		End Property
	End Class
End Namespace