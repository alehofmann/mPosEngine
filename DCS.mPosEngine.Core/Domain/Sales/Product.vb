Imports DCS.ProjectBase.Core

Namespace Domain.Sales
    Public Class Product
        Inherits DomainObject(Of Long)

        Private _posProduct As DCS.PlaycardBase.Core.PosDomain.Product
        Private _thumbUrl As String
        Private _needsQuantity As Boolean
        Private _securityActionId As Integer
        Private _forceReenterCredentials As Boolean
        Private _deleted As Boolean

        Public Property Deleted() As Boolean
            Get
                Return _deleted
            End Get
            Set(ByVal value As Boolean)
                _deleted = value
            End Set
        End Property

        Public ReadOnly Property IsDefaultCardSellProduct As Boolean
            Get
                Return (_posProduct.Id = 1)
            End Get
        End Property

        Public Property ForceReenterCredentials() As Boolean
            Get
                Return _forceReenterCredentials
            End Get
            Set(ByVal value As Boolean)
                _forceReenterCredentials = value
            End Set
        End Property

        Public Property SecurityActionId() As Integer
            Get
                Return _SecurityActionId
            End Get
            Set(ByVal value As Integer)
                _SecurityActionId = value
            End Set
        End Property


        Public Overridable Property ThumbUrl() As String
            Get
                Return _thumbUrl
            End Get
            Set(ByVal value As String)
                _thumbUrl = value
            End Set
        End Property

        Public Overridable Property NeedsQuantity() As Boolean
            Get
                Return _needsQuantity
            End Get
            Set(ByVal value As Boolean)
                _needsQuantity = value
            End Set
        End Property

        Public Property ProductData() As DCS.PlaycardBase.Core.PosDomain.Product
            Get
                Return _posProduct
            End Get
            Set(ByVal value As DCS.PlaycardBase.Core.PosDomain.Product)
                _posProduct = value
            End Set
        End Property



        Public Overrides Function GetHashCode() As Integer
            Return (GetType(Product).FullName & _posProduct.GetHashCode).GetHashCode
        End Function
    End Class
End Namespace
