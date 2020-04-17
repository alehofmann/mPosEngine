Imports DCS.mPosEngine.Services.Dto
Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto
	<JsonObject()>
	<DataContract()>
	Public Class CommitTransactionCommandDto
		Private _payitems As IList(Of PayItemDto) = New List(Of PayItemDto)
		Private _transactionCart As TransactionCartDto
		Private _operatorId As Integer
		Private _posName As String
		Private _invoiceNumber As String

		<DataMember(Name:="invoiceNumber")>
		Public Property InvoiceNumber() As String
			Get
				Return _invoiceNumber
			End Get
			Set(ByVal value As String)
				_invoiceNumber = value
			End Set
		End Property

		<DataMember(Name:="posName")>
		Public Property PosName() As String
			Get
				Return _posName
			End Get
			Set(ByVal value As String)
				_posName = value
			End Set
		End Property

		Public Sub Validate()
            'If _transactionCart Is Nothing OrElse _transactionCart.LineItems.Count = 0 Then
            '	Throw New InvalidDtoException(1, "A non-empty shopping cart must be supplied in CommitTransactionCommandDto")
            'End If

            If _operatorId = 0 Then
				Throw New InvalidDtoException(2, "Operator Id must not be null in CommitTransactoinCommandDto")
			End If

			If _posName = "" Then
				Throw New InvalidDtoException(2, "Pos Id must not be null in CommitTransactoinCommandDto")
			End If
		End Sub
		<JsonProperty()>
		<DataMember(Name:="operatorId")>
		Public Property OperatorId() As Integer
			Get
				Return _operatorId
			End Get
			Set(ByVal value As Integer)
				_operatorId = value
			End Set
		End Property

		<JsonProperty()>
		<DataMember(Name:="transactionCart")>
		Public Property Cart() As TransactionCartDto
			Get
				Return _transactionCart
			End Get
			Set(ByVal value As TransactionCartDto)
				_transactionCart = value
			End Set
		End Property

		<JsonProperty()>
		<DataMember(Name:="payment")>
		Public Property Payitems() As IList(Of PayItemDto)
			Get
				Return _payitems
			End Get
			Set(ByVal value As IList(Of PayItemDto))
				_payitems = value
			End Set
		End Property

		<JsonProperty()>
        <DataMember(Name:="discountsApplied")>
        Public Property DiscountsApplied As String

        <JsonProperty()>
        <DataMember(Name:="vouchers")>
        Public Property Vouchers As String
    End Class
End Namespace