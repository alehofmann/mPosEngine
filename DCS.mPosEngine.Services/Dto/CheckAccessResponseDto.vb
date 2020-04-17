Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto

    <JsonObject()> _
    <DataContract()> _
    Public Class CheckAccessResponseDto
        Public Enum ResultCodesEnum
            AccessGranted = 0
            AccessDenied = 1
        End Enum
        Private _resultCode As ResultCodesEnum

        <DataMember(Name:="resultCode")> _
        <JsonProperty()> _
        Public Property ResultCode() As ResultCodesEnum
            Get
                Return _resultCode
            End Get
            Set(ByVal value As ResultCodesEnum)
                _resultCode = value
            End Set
        End Property

        Public Sub New()

        End Sub
        Public Sub New(ByVal accessGranted As Boolean)
            _resultCode = IIf(accessGranted, ResultCodesEnum.AccessGranted, ResultCodesEnum.AccessDenied)
        End Sub
    End Class
End Namespace
