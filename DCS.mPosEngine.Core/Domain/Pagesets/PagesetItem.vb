Namespace Domain.Pagesets
    Public Class PagesetItem
        Private _page As ProductPage
        Private _displayOrder As Integer

        Public Property DisplayOrder() As Integer
            Get
                Return _displayOrder
            End Get
            Set(ByVal value As Integer)
                _displayOrder = value
            End Set
        End Property

        Public Property Page() As ProductPage
            Get
                Return _page
            End Get
            Set(ByVal value As ProductPage)
                _page = value
            End Set
        End Property


            End Class
End Namespace
