Imports DCS.ProjectBase.Core
Imports DCS.mPosEngine.Core.Domain.Sales

Namespace Domain.Pagesets
    Public Class ProductPage
        Inherits DomainObject(Of Integer)

        Private _pageName As String
		Private _products As ISet(Of Product) = New HashSet(Of Product)

		Public Property Products() As ISet(Of Product)
			Get
				Return _products
			End Get
			Set(ByVal value As ISet(Of Product))
				_products = value
			End Set
		End Property

        
        Public Property PageName() As String
            Get
                Return _pageName
            End Get
            Set(ByVal value As String)
                _pageName = value
            End Set
        End Property


        Public Overrides Function GetHashCode() As Integer
            Return (GetType(ProductPage).FullName & Id.GetHashCode).GetHashCode
        End Function
    End Class
End Namespace
