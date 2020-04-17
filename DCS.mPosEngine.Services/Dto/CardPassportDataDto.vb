Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto
    <JsonObject()> _
    <DataContract()> _
    Public Class CardPassportDataDto
        Private _id As Integer
        Private _name As String
        Private _quantity As Decimal
        Private _enabled As Boolean


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

        <DataMember(Name:="enabled")> _
        <JsonProperty()> _
        Public Property Enabled() As Boolean
            Get
                Return _enabled
            End Get
            Set(ByVal value As Boolean)
                _enabled = value
            End Set
        End Property

        <DataMember(Name:="qty")> _
        <JsonProperty()> _
        Public Property Quantity() As Decimal
            Get
                Return _quantity
            End Get
            Set(ByVal value As Decimal)
                _quantity = value
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
    End Class
End Namespace
