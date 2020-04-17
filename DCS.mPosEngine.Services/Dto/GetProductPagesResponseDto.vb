Imports System.Runtime.Serialization

Namespace Dto
    <DataContract()>
    Public Class GetProductPagesResponseDto

        Private _productPages As IList(Of ProductPageDto) = New List(Of ProductPageDto)
        Private _cardProduct As ProductDto

        <DataMember(Name:="defaultCardProduct")> _
        Public Property CardProduct() As ProductDto
            Get
                Return _cardProduct
            End Get
            Set(ByVal value As ProductDto)
                _cardProduct = value
            End Set
        End Property

        <DataMember(Name:="productPages")> _
        Public Property ProductPages() As IList(Of ProductPageDto)
            Get
                Return _productPages
            End Get
            Set(ByVal value As IList(Of ProductPageDto))
                _productPages = value
            End Set
        End Property

    End Class
End Namespace
