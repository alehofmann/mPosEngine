Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto
    <JsonObject()>
    <DataContract()>
    Public Class InvoiceImageResponseDto
        Public Enum ResultCode
            Ok = 0
            PathNotFound = 1
            FileNotFound = 2
            UnknownError = 3
        End Enum

        <DataMember(Name:="result")>
        <JsonProperty()>
        Public Property Result As ResultCode

        <DataMember(Name:="image")>
        <JsonProperty()>
        Public Property Image As String

        Public Sub New(res As ResultCode, imageString As String)
            Result = res
            Image = imageString
        End Sub

        Public Sub New(res As ResultCode)
            Result = res
            Image = String.Empty
        End Sub
    End Class
End Namespace