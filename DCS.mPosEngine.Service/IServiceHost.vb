Imports System.ServiceModel
Imports System.ServiceModel.Web
Imports DCS.mPosEngine.WebService.CommObjects
Imports DCS.mPosEngine.Services.Dto
Imports System.Runtime.Serialization

<ServiceContract()>
Public Interface IServiceHost
	'<WebGet(ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.WrappedRequest)>
	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function GetProducts(ByVal userCardNumber As Integer) As CommandResponse(Of IList(Of ProductDto))

	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function GetProductPages(ByVal posId As String, ByVal userCardNumber As Integer) As CommandResponse(Of GetProductPagesResponseDto)

	'<WebGet(ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.WrappedRequest)>
	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function GetPaymodes() As CommandResponse(Of IList(Of PaymodeDto))


	'<WebGet(ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.WrappedRequest)>
	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function AnalyzeCard(ByVal cardNumber As Integer) As CommandResponse(Of CardDataDto)

	'<WebGet(ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.WrappedRequest)>
	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function GetCardHistory(ByVal cardNumber As Integer, ByVal maxLines As Integer) As CommandResponse(Of IList(Of CardHistoryLineDto))

	'<WebGet(ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.WrappedRequest)>
	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function LoginCashier(ByVal loginMode As Integer, ByVal cardNumber As Integer, ByVal userId As String, ByVal password As String) As CommandResponse(Of LoginDataDto)

	'<WebGet(ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.WrappedRequest)>
	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function GetPosConfig(ByVal posName As String) As CommandResponse(Of IList(Of PosConfigDto))

	'<WebGet(ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.WrappedRequest)>
	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function CheckDevice(ByVal deviceSerial As String) As CommandResponse(Of CheckDeviceResponseDto)

	'<WebGet(ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.WrappedRequest)>
	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function RegisterDevice(ByVal deviceSerial As String, ByVal pairCode As String) As CommandResponse(Of RegisterDeviceResponseDto)

	'<WebGet(ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.WrappedRequest)>
	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function CheckAccess(ByVal actionId As Integer, ByVal userCardNumber As Integer) As CommandResponse(Of CheckAccessResponseDto)

	'<WebGet(ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.WrappedRequest)>
	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function GetAdminFunctions(ByVal userCardNumber As Integer) As CommandResponse(Of IList(Of AdminFunctionDto))

	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function GetTransactionInfo(ByVal transactionCart As TransactionCartDto, discountsApplied As String, posName As String) As CommandResponse(Of TransactionInfoDto)

	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function GetCardStatus(ByVal cardNumber As Integer) As CommandResponse(Of CardStatusDto)

	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.Bare, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function CommitTransaction(ByVal command As CommitTransactionCommandDto) As CommandResponse(Of CommitTransactionResponseDto)

	'<WebGet(ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.WrappedRequest)>
	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.Bare, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function CardTransfer(ByVal command As CardTransferCommandDto) As CommandResponse(Of CardTransferResponseDto)

	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.Bare, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function CardConsolidate(ByVal command As CardConsolidateCommandDto) As CommandResponse(Of CardTransferResponseDto)

	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.Bare, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function AuthorizePayment(ByVal paymentData As IList(Of PayItemDto)) As CommandResponse(Of AuthorizePaymentResponseDto)

	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.Bare, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function GetDiscountsFromProducts(ByVal productsIds As TransactionCartDto) As CommandResponse(Of List(Of DiscountDto))

	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function CashPull(cashierId As Integer, cash As List(Of MoneyOperationDto)) As CommandResponse(Of SuccessResponse)

	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function PayOut(cashierId As Integer, reason As String, cash As List(Of MoneyOperationDto)) As CommandResponse(Of SuccessResponse)

	<WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function GetBalance(cashierId As Integer) As CommandResponse(Of TreasuryBalanceDto)

    <WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
	<OperationContract()>
	Function GetCreditCardTransactionStatus(logCreditCardTransactionId As integer) As CommandResponse(Of CreditCardStatusDto)

    <WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
    <OperationContract()>
    Function ReturnCreditCardTransaction(invoiceId As Long, logCreditCardTransactionId As Integer, purchaseAmount As Decimal) As CommandResponse(Of ReturnCreditCardResponseDto)

    <WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
    <OperationContract()>
    Function StartCreditCardAuthorizer(provider As String, config As IList(Of KeyValuePair(Of String, String))) As CommandResponse(Of GenericResultResponseDto)

    <WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
    <OperationContract()>
    Function CheckVoucher(voucherCode As String) As CommandResponse(Of VoucherResponseDto)

    <WebInvoke(Method:="POST", BodyStyle:=WebMessageBodyStyle.WrappedRequest, ResponseFormat:=WebMessageFormat.Json, RequestFormat:=WebMessageFormat.Json)>
    <OperationContract()>
    Function GetInvoiceImage(filename As String) As CommandResponse(Of InvoiceImageResponseDto)
End Interface

<DataContract()> _
Public Class LoginData
    Public LoginMode As Integer
    Public CardNumber As Integer
    Public UserId As String
    Public Password As String
End Class