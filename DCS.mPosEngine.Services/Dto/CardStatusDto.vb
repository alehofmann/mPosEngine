Imports Newtonsoft.Json
Imports System.Runtime.Serialization

Namespace Dto

    <JsonObject()> _
    <DataContract()>
    Public Class CardStatusDto
        Private _cardStatus As CardStatusesEnum

        <DataMember(Name:="cardStatus")> _
        <JsonProperty()> _
        Public Property CardStatus() As CardStatusesEnum
            Get
                Return _cardStatus
            End Get
            Set(ByVal value As CardStatusesEnum)
                _cardStatus = value
            End Set
        End Property

        Public Enum CardStatusesEnum
            ValidSoldCard = 1
            NewCard = 2
            InvalidCardNumber = 3
            ServiceCard = 4
        End Enum
    End Class
End Namespace
