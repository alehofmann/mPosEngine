Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto
    <JsonObject()> _
    <DataContract()>
    Public Class CommandResponseDto
        Private _commandSuccess As Boolean
        Private _errorDescription As String

        <JsonProperty("errorDescription")> _
        <DataMember(Name:="errorDescription")> _
        Public Property ErrorDescription() As String
            Get
                Return _errorDescription
            End Get
            Set(ByVal value As String)
                _errorDescription = value
            End Set
        End Property

        <JsonProperty("commandSuccess")> _
        <DataMember(Name:="commandSuccess")> _
        Public Property CommandSuccess() As Boolean
            Get
                Return _commandSuccess
            End Get
            Set(ByVal value As Boolean)
                _commandSuccess = value
            End Set
        End Property

    End Class
End Namespace