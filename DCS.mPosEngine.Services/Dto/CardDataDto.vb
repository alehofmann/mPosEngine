Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto

    <JsonObject()>
    <DataContract()>
    Public Class CardDataDto
        Private _cardNumber As Long
        Private _cardStatus As Char
        Private _credits As Decimal
        Private _bonus As Decimal
        Private _courtesy As Decimal
        Private _tickets As Integer

        Private _minutes As Integer

        <DataMember(Name:="minutes")>
        <JsonProperty()>
        Public Property Minutes() As Integer
            Get
                Return _minutes
            End Get
            Set(ByVal value As Integer)
                _minutes = value
            End Set
        End Property

        <DataMember(Name:="passports")>
        <JsonProperty()>
        Private _passports As IList(Of PassportItemDto) = New List(Of PassportItemDto)

        Public Property Passports() As IList(Of PassportItemDto)
            Get
                Return _passports
            End Get
            Set(ByVal value As IList(Of PassportItemDto))
                _passports = value
            End Set
        End Property


        <DataMember(Name:="cardNro")>
        <JsonProperty()>
        Public Property CardNumber() As Long
            Get
                Return _cardNumber
            End Get
            Set(ByVal value As Long)
                _cardNumber = value
            End Set
        End Property

        <DataMember(Name:="status")>
        <JsonProperty()>
        Public Property CardStatus() As Char
            Get
                Return _cardStatus
            End Get
            Set(ByVal value As Char)
                _cardStatus = value
            End Set
        End Property

        <DataMember(Name:="credits")>
        <JsonProperty()>
        Public Property Credits() As Decimal
            Get
                Return _credits
            End Get
            Set(ByVal value As Decimal)
                _credits = value
            End Set
        End Property

        <DataMember(Name:="bonus")>
        <JsonProperty()>
        Public Property Bonus() As Decimal
            Get
                Return _bonus
            End Get
            Set(ByVal value As Decimal)
                _bonus = value
            End Set
        End Property

        <DataMember(Name:="courtesy")>
        <JsonProperty()>
        Public Property Courtesy() As Decimal
            Get
                Return _courtesy
            End Get
            Set(ByVal value As Decimal)
                _courtesy = value
            End Set
        End Property

        <DataMember(Name:="tickets")>
        <JsonProperty()>
        Public Property Tickets() As Integer
            Get
                Return _tickets
            End Get
            Set(ByVal value As Integer)
                _tickets = value
            End Set
        End Property

        <DataMember(Name:="playAmountToPromote")>
        <JsonProperty()>
        Public Property PlayAmountToPromote() As Decimal
    End Class
End Namespace