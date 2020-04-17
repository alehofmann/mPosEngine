Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto
    <JsonObject()> _
    <DataContract()>
    Public Class PaymodeDto
        Private _id As Integer
        Private _name As String
        Private _type As Integer


        <DataMember(Name:="type")> _
        <JsonProperty()> _
        Public Property Type() As Integer
            Get
                Return _type
            End Get
            Set(ByVal value As Integer)
                _type = value
            End Set
        End Property

        <DataMember(Name:="name")> _
        <JsonProperty()> _
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        <DataMember(Name:="id")> _
        <JsonProperty()> _
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property
    End Class
End Namespace
