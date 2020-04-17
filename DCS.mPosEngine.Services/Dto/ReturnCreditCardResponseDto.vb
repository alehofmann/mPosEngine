Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto
    <JsonObject()>
    <DataContract()>
    Public Class ReturnCreditCardResponseDto
        Public Enum ResultCodes
            ReturnInProgress = 0
            PaymentInProgress = 1
            TransactionNotFound = 2
            UndefinedError = 3
        End Enum

        Public Sub New(result As ResultCodes, Optional transactionId As Long = 0)
            ResultCode = result
            ReturnTransactionId = transactionId
        End Sub

        <JsonProperty()>
        <DataMember(Name:="resultCode")>
        Public Property ResultCode As ResultCodes

        <JsonProperty()>
        <DataMember(Name:="returnTransactionId")>
        Public Property ReturnTransactionId As long
    End Class
End Namespace