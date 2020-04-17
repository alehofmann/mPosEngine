Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto
    <JsonObject()>
    <DataContract()>
    Public Class GenericResultResponseDto
        <DataMember(Name := "resultCode")>
        public Property ResultCode As Integer

        <DataMember(Name := "resultMessage")>
        Public Property ResultMessage As string

        Public sub New()
        End sub

        public sub New(code As Integer, msg As string)
            ResultCode = code
            ResultMessage = msg
        End sub
    End Class
End Namespace