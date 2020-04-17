Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto

    <JsonObject()> _
    <DataContract()>
    Public Class CardHistoryLineDto
        Private _transDate As Date
        Private _entity As String
        Private _description As String
        Private _counter As String
        Private _amount As String

        <DataMember(Name:="amount")> _
        <JsonProperty()> _
        Public Property Amount() As String
            Get
                Return _amount
            End Get
            Set(ByVal value As String)
                _amount = value
            End Set
        End Property

        <DataMember(Name:="counter")> _
        <JsonProperty()> _
        Public Property Counter() As String
            Get
                Return _counter
            End Get
            Set(ByVal value As String)
                _counter = value
            End Set
        End Property

        <DataMember(Name:="description")> _
        <JsonProperty()> _
        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        <DataMember(Name:="dateTime")> _
        Public Property FormattedDate As String
            Get
                Return _transDate.ToString("g")
            End Get
            Private Set(ByVal value As String)

            End Set
        End Property


        Public Property TransDate() As Date
            Get
                Return _transDate
            End Get
            Set(ByVal value As Date)
                _transDate = value
            End Set
        End Property

        <DataMember(Name:="entity")> _
        <JsonProperty()> _
        Public Property Entity() As String
            Get
                Return _entity
            End Get
            Set(ByVal value As String)
                _entity = value
            End Set
        End Property
    End Class
End Namespace
