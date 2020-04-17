Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto
    <JsonObject()> _
    <DataContract()>
    Public Class PosConfigDto
        Private _section As String
        Private _key As String
        Private _value As String


        <DataMember(Name:="section")> _
        <JsonProperty()> _
        Public Property Section() As String
            Get
                Return _section
            End Get
            Set(ByVal value As String)
                _section = value
            End Set
        End Property

        <DataMember(Name:="key")> _
        <JsonProperty()> _
        Public Property Key() As String
            Get
                Return _key
            End Get
            Set(ByVal value As String)
                _key = value
            End Set
        End Property

        <DataMember(Name:="value")> _
        <JsonProperty()> _
        Public Property Value() As String
            Get
                Return _value
            End Get
            Set(ByVal theValue As String)
                _value = theValue
            End Set
        End Property
    End Class
End Namespace
