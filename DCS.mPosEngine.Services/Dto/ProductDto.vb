Imports Newtonsoft.Json
Imports System.Runtime.Serialization

Namespace Dto


    <JsonObject()> _
    <DataContract()> _
    Public Class ProductDto

        Private _id As Integer
        Private _name As String
        Private _taxBase As Decimal
        Private _needsQuantity As Boolean
        Private _thumbUrl As String
        Private _price As Decimal
        Private _authenticationRequired As Boolean
        Private _cardRequired As Boolean
        Private _cardIncluded As Boolean
        Private _securityActionId As Integer

        Private _isDefaultCardSellProduct As Boolean

        <DataMember(Name:="isDefaultCardSellProduct")> _
        Public Property IsDefaultCardSellProduct() As Boolean
            Get
                Return _isDefaultCardSellProduct
            End Get
            Set(ByVal value As Boolean)
                _isDefaultCardSellProduct = value
            End Set
        End Property

        <DataMember(Name:="securityActionId")> _
        Public Property SecurityActionId() As Integer
            Get
                Return _securityActionId
            End Get
            Set(ByVal value As Integer)
                _securityActionId = value
            End Set
        End Property

        <DataMember(Name:="cardIncluded")> _
        Public Property CardIncluded() As Boolean
            Get
                Return _cardIncluded
            End Get
            Set(ByVal value As Boolean)
                _cardIncluded = value
            End Set
        End Property

        <DataMember(Name:="cardRequired")> _
        Public Property CardRequired() As Boolean
            Get
                Return _CardRequired
            End Get
            Set(ByVal value As Boolean)
                _CardRequired = value
            End Set
        End Property


        <DataMember(Name:="authenticationRequired")> _
        <JsonProperty()> _
        Public Property AuthenticationRequired() As Boolean

            Get
                Return _authenticationRequired
            End Get
            Set(ByVal value As Boolean)
                _authenticationRequired = value
            End Set
        End Property


        Public Sub New(ByVal id As Integer, ByVal name As String, ByVal taxBase As Decimal, ByVal needsQuantity As Boolean, ByVal thumbUrl As String, ByVal price As Decimal)
            _id = id
            _name = name
            _taxBase = taxBase
            _needsQuantity = needsQuantity
            _thumbUrl = thumbUrl
            _price = price
        End Sub

        <DataMember(Name:="taxBase")> _
        <JsonProperty()> _
        Public Property TaxBase() As Decimal
            Get
                Return _taxBase
            End Get
            Set(ByVal value As Decimal)
                _taxBase = value
            End Set
        End Property

        <DataMember(Name:="needsQuantity")> _
        <JsonPropertyAttribute()> _
        Public Property NeedsQuantity() As Boolean
            Get
                Return _needsQuantity
            End Get
            Set(ByVal value As Boolean)
                _needsQuantity = value
            End Set
        End Property

        <DataMember(Name:="thumbUrl")> _
        <JsonPropertyAttribute()> _
        Public Property ThumbUrl() As String
            Get
                Return _thumbUrl
            End Get
            Set(ByVal value As String)
                _thumbUrl = value
            End Set
        End Property
        <DataMember(Name:="price")> _
        <JsonPropertyAttribute()> _
        Public Property Price() As Decimal
            Get
                Return _price
            End Get
            Set(ByVal value As Decimal)
                _price = value
            End Set
        End Property

        <DataMember(Name:="name")> _
        <JsonPropertyAttribute()> _
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        <DataMember(Name:="id")> _
        <JsonPropertyAttribute()> _
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