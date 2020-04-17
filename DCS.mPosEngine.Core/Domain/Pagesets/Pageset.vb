Imports DCS.ProjectBase.Core

Namespace Domain.Pagesets
    Public Class Pageset
        Inherits DomainObject(Of Integer)

        Private _name As String
        Private _items As ISet(Of PagesetItem)

        Public Property Items() As ISet(Of PagesetItem)
            Get
                Return _items
            End Get
            Set(ByVal value As ISet(Of PagesetItem))
                _items = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property


        Public Overrides Function GetHashCode() As Integer

        End Function
    End Class
End Namespace
