Imports DCS.mPosEngine.Core.Domain.Sales

Namespace Domain.Pagesets
    Public Class ProductButton        
        Private _product As Product
        Private _displayOrder As Integer
        Private _buttonSize As Integer

        Private _page As ProductPage

        Public Property Page() As ProductPage
            Get
                Return _page
            End Get
            Set(ByVal value As ProductPage)
                _page = value
            End Set
        End Property

        Public Property ButtonSize() As Integer
            Get
                Return _buttonSize
            End Get
            Set(ByVal value As Integer)
                _buttonSize = value
            End Set
        End Property

        Public Property DisplayOrder() As Integer
            Get
                Return _displayOrder
            End Get
            Set(ByVal value As Integer)
                _displayOrder = value
            End Set
        End Property

        Public Property Product() As Product
            Get
                Return _product
            End Get
            Set(ByVal value As Product)
                _product = value
            End Set
        End Property
        Public Overrides Function Equals(obj As Object) As Boolean
            If obj Is Nothing Then
                Return False
            End If

            Dim other As ProductButton
            other = CType(obj, ProductButton)

            If other Is Nothing Then
                Return False
            End If

            If GetHashCode() = other.GetHashCode Then
                Return True
            End If
            Return False

        End Function
        Public Overrides Function GetHashCode() As Integer
            Return (GetType(ProductButton).FullName & Product.GetHashCode & Page.GetHashCode).GetHashCode
        End Function
    End Class
End Namespace
