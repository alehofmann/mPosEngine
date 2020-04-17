Imports Newtonsoft.Json
Imports System.Runtime.Serialization

Namespace Dto
    <JsonObject()> _
    <DataContract()> _
    Public Class TaxInfoItemDto
        Private _taxName As String
        Private _taxAmount As Decimal
        Public Property TaxAmount() As String
            Get
                Return _taxAmount
            End Get
            Set(ByVal value As String)
                _taxAmount = value
            End Set
        End Property

        Public Property TaxName() As String
            Get
                Return _taxName
            End Get
            Set(ByVal value As String)
                _taxName = value
            End Set
        End Property

    End Class
End Namespace
