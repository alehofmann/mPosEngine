Imports Newtonsoft.Json
Imports System.Runtime.Serialization

Namespace Dto
	<JsonObject()>
	<DataContract()>
	Public Class LineitemDto
		Private _productId As Long
		Private _quantity As Decimal
		Private _unitPrice As Decimal
		Private _cardNumber As Long
		Private _externalSystemData As ExternalSystemDataDto

		Public Sub New()
		End Sub

		Public Sub New(ByVal productId As Long, ByVal quantity As Decimal, ByVal unitPrice As Decimal, ByVal cardNumber As Long)
			_productId = productId
			_quantity = quantity
			_unitPrice = unitPrice
			_cardNumber = cardNumber
		End Sub

		<DataMember(Name:="externalPayment")>
		Public Property ExternalSystemData() As ExternalSystemDataDto
			Get
				Return _externalSystemData
			End Get
			Set(ByVal value As ExternalSystemDataDto)
				_externalSystemData = value
			End Set
		End Property

		<DataMember(Name:="cardNumber")>
		Public Property CardNumber() As Long
			Get
				Return _cardNumber
			End Get
			Set(ByVal value As Long)
				_cardNumber = value
			End Set
		End Property

		<DataMember(Name:="unitPrice")>
		<JsonProperty()>
		Public Property UnitPrice() As Decimal
			Get
				Return _unitPrice
			End Get
			Set(ByVal value As Decimal)
				_unitPrice = value
			End Set
		End Property

		<DataMember(Name:="quantity")>
		<JsonProperty()>
		Public Property Quantity() As Decimal
			Get
				Return _quantity
			End Get
			Set(ByVal value As Decimal)
				_quantity = value
			End Set
		End Property

		<DataMember(Name:="productId")>
		<JsonProperty()>
		Public Property ProductId() As Long
			Get
				Return _productId
			End Get
			Set(ByVal value As Long)
				_productId = value
			End Set
		End Property
	End Class
End Namespace