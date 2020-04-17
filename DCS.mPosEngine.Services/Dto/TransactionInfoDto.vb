Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto
	<JsonObject()>
	<DataContract()>
	Public Class TransactionInfoDto
		Private _subtotal1 As Decimal
		Private _subtotal2 As Decimal
		Private _totalTax As Decimal
		Private _totalDiscount As Decimal
		Private _taxDetail As IList(Of TaxInfoItemDto) = New List(Of TaxInfoItemDto)
		Private _invoiceNumber As String

		<DataMember(Name:="invoiceNumber")>
		<JsonProperty()>
		Public Property InvoiceNumber() As String
			Get
				Return _invoiceNumber
			End Get
			Set(ByVal value As String)
				_invoiceNumber = value
			End Set
		End Property

		<DataMember(Name:="taxDetail")>
		<JsonProperty()>
		Public Property TaxDetail() As IList(Of TaxInfoItemDto)
			Get
				Return _taxDetail
			End Get
			Set(ByVal value As IList(Of TaxInfoItemDto))
				_taxDetail = value
			End Set
		End Property

		Private _totalToPay As Decimal

		<DataMember(Name:="totalToPay")>
		<JsonProperty()>
		Public Property TotalToPay() As Decimal
			Get
				Return _totalToPay
			End Get
			Set(ByVal value As Decimal)
				_totalToPay = value
			End Set
		End Property

		<DataMember(Name:="totalDiscount")>
		<JsonProperty()>
		Public Property TotalDiscount() As Decimal
			Get
				Return _totalDiscount
			End Get
			Set(ByVal value As Decimal)
				_totalDiscount = value
			End Set
		End Property

		<DataMember(Name:="totalTax")>
		<JsonProperty()>
		Public Property TotalTax() As Decimal
			Get
				Return _totalTax
			End Get
			Set(ByVal value As Decimal)
				_totalTax = value
			End Set
		End Property

		<DataMember(Name:="subtotal2")>
		<JsonProperty()>
		Public Property Subtotal2() As Decimal
			Get
				Return _subtotal2
			End Get
			Set(ByVal value As Decimal)
				_subtotal2 = value
			End Set
		End Property

		<DataMember(Name:="subtotal1")>
		<JsonProperty()>
		Public Property Subtotal1() As Decimal
			Get
				Return _subtotal1
			End Get
			Set(ByVal value As Decimal)
				_subtotal1 = value
			End Set
		End Property

		'<DataMember(Name:="discounts")>
		'<JsonProperty()>
		'Public Property Discounts() As IList(Of DiscountDto)
	End Class
End Namespace