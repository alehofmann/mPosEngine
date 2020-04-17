Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto
    <JsonObject()> _
    <DataContract()>
    Public Class AuthorizePaymentResponseDto
        Private _resultCode As ResultCodesEnum

        Public Enum ResultCodesEnum
            PaymentAuthorized = 0
            NotEnoughCreditsOnPlaycard = 1
            UnsoldPlaycard = 2
            UndefinedError = 3
        End Enum

        Public Sub New(resultCode As ResultCodesEnum)
            _resultCode = resultCode
            TransactionId = 0
        End Sub

        Public Sub New(resultCode As ResultCodesEnum, transId As Integer)
            _resultCode = resultCode
            TransactionId = transId
        End Sub

        <JsonProperty()>
        <DataMember(Name:="resultCode")>
        Public Property ResultCode As ResultCodesEnum
            Get
                Return _resultCode
            End Get
            Set(value As ResultCodesEnum)
                _resultCode = value
            End Set
        End Property

        <JsonProperty()>
        <DataMember(Name:="transactionId")>
        Public Property TransactionId As Integer
    End Class
End Namespace