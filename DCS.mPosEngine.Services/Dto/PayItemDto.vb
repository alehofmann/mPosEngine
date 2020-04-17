Imports System.ComponentModel
Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto
	<DataContract()>
	Public Class PayItemDto
		Private _paymodeId As Integer
		Private _amount As Decimal
        Private _gratuity As Decimal
		Private _playcardNumber As Long
		Private _creditCardNumber As String
		Private _creditCardType As String
		Private _creditCardAuthorizationReference As String
		Private _creditCardDebugInfo As String

		Public Sub New()
		End Sub

		Public Sub New(ByVal paymodeId As Integer, ByVal amount As Decimal, ByVal playcardNumber As Long, ByVal creditCardNumber As String, ByVal creditCardType As String, ByVal authReference As String, ByVal debugInfo As String)
			_paymodeId = paymodeId
			_amount = amount
			_playcardNumber = playcardNumber
			_creditCardNumber = creditCardNumber
			_creditCardType = creditCardType
			_creditCardAuthorizationReference = authReference
			_creditCardDebugInfo = debugInfo
            _gratuity = 0
		End Sub

        Public Sub New(ByVal paymodeId As Integer, ByVal amount As Decimal, ByVal playcardNumber As Long, ByVal creditCardNumber As String, ByVal creditCardType As String, ByVal authReference As String, ByVal debugInfo As String, ByVal gratuity As Decimal)
			_paymodeId = paymodeId
			_amount = amount
			_playcardNumber = playcardNumber
			_creditCardNumber = creditCardNumber
			_creditCardType = creditCardType
			_creditCardAuthorizationReference = authReference
			_creditCardDebugInfo = debugInfo
            _gratuity = gratuity
		End Sub

		<DataMember(Name:="creditCardDebugInfo")>
		Public Property CreditCardDebugInfo() As String
			Get
				Return _creditCardDebugInfo
			End Get
			Set(ByVal value As String)
				_creditCardDebugInfo = value
			End Set
		End Property

		<DataMember(Name:="creditCardAuthorizationReference")>
		Public Property CreditCardAuthorizationReference() As String
			Get
				Return _creditCardAuthorizationReference
			End Get
			Set(ByVal value As String)
				_creditCardAuthorizationReference = value
			End Set
		End Property

		<DataMember(Name:="creditCardType")>
		Public Property CreditCardType() As String
			Get
				Return _creditCardType
			End Get
			Set(ByVal value As String)
				_creditCardType = value
			End Set
		End Property

		<DataMember(Name:="creditCardNumber")>
		Public Property CreditCardNumber() As String
			Get
				Return _creditCardNumber
			End Get
			Set(ByVal value As String)
				_creditCardNumber = value
			End Set
		End Property

		<DataMember(Name:="playcardNumber", EmitDefaultValue := True)>
		Public Property PlaycardNumber() As Long
			Get
				Return _playcardNumber
			End Get
			Set(ByVal value As Long)
				_playcardNumber = value
			End Set
		End Property

		<DataMember(Name:="amount")>
		Public Property Amount() As Decimal
			Get
				Return _amount
			End Get
			Set(ByVal value As Decimal)
				_amount = value
			End Set
		End Property

        <DefaultValue(0)>
        <JsonProperty(DefaultValueHandling := DefaultValueHandling.Populate)>
        <DataMember(Name:="gratuity")>
		Public Property Gratuity() As Decimal
			Get
				Return _gratuity
			End Get
			Set(ByVal value As Decimal)
				_gratuity = value
			End Set
		End Property

		<DataMember(Name:="paymodeId")>
		Public Property PaymodeId() As Integer
			Get
				Return _paymodeId
			End Get
			Set(ByVal value As Integer)
				_paymodeId = value
			End Set
		End Property

        <DataMember(Name:="invoiceNo")>
		Public Property InvoiceNo() As String
	End Class
End Namespace