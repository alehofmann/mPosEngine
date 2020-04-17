Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto

    <JsonObject()> _
    <DataContract()> _
    Public Class ExternalSystemDataDto
        Private _externalSystemId As Integer
        'Private _params As IList(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))
        Private _params As Dictionary(Of String, String)

        <JsonProperty()> _
        <DataMember(Name:="params")> _
        Public Property Params() As Dictionary(Of String, String)
            Get
                Return _params
            End Get
            Set(ByVal value As Dictionary(Of String, String))
                _params = value
            End Set
        End Property

        <JsonProperty()> _
        <DataMember(Name:="externalSystemId")> _
        Public Property ExternalSystemId As Integer
            Get
                Return _externalSystemId
            End Get
            Set(value As Integer)
                _externalSystemId = value
            End Set
        End Property
    End Class
End Namespace
