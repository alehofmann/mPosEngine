Imports System.Runtime.Serialization
Imports DCS.mPosEngine.Services.Payment
Imports Newtonsoft.Json

Namespace Dto
    <JsonObject()>
    <DataContract()>
    Public Class CreditCardStatusDto
		<JsonProperty()>
        <DataMember(Name:="status")>
        Public Property Status As Integer

		<JsonProperty()>
        <DataMember(Name:="errorCode")>
        Public property ErrorCode As Integer

		<JsonProperty()>
        <DataMember(Name:="errorMessage")>
        Public Property ErrorMessage As string

		<JsonProperty()>
        <DataMember(Name:="creditCardNumber")>
        Public Property CreditCardNumber As String

		<JsonProperty()>
        <DataMember(Name:="cardType")>
        Public Property CreditCardType As String

		<JsonProperty()>
        <DataMember(Name:="printData")>
        Public Property PrintData As string()

        Public sub New(cmdStatus As PaymentService.CcardTransactionStatus, errCode As Integer, errorMsg As String, printText As string, optional creditCardNo As String = "", optional cardType As string = "")
            Status = cmdstatus
            ErrorCode = errCode
            ErrorMessage = errorMsg

            if Not String.IsNullOrWhiteSpace(printText)
                PrintData = printText.Split("|"C)
            else
                PrintData = Nothing
            End If

            CreditCardNumber = creditCardNo.Trim()
            CreditCardType = cardType.Trim()
        End sub
    End Class
End Namespace