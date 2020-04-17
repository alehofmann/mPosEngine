Imports System.Runtime.Serialization

Namespace Dto
    <DataContract()>
    Public Class ProductPageDto
        Private _pageName As String
        Private _displayOrder As Integer

        Private _productList As IList(Of ProductDto) = New List(Of ProductDto)

        <DataMember(Name:="productList")>
        Public Property ProductList() As IList(Of ProductDto)
            Get
                Return _productList
            End Get
            Set(ByVal value As IList(Of ProductDto))
                _productList = value
            End Set
        End Property

        <DataMember(Name:="displayOrder")>
        Public Property DisplayOrder() As Integer
            Get
                Return _displayOrder
            End Get
            Set(ByVal value As Integer)
                _displayOrder = value
            End Set
        End Property

        <DataMember(Name:="pageName")>
        Public Property PageName() As String
            Get
                Return _pageName
            End Get
            Set(ByVal value As String)
                _pageName = value
            End Set
        End Property

    End Class
End Namespace
