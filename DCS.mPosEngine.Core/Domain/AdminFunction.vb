Imports DCS.ProjectBase.Core

Namespace Domain
    Public Class AdminFunction
        Inherits DomainObject(Of Integer)

        Private _securityActionId As Integer
        Private _forceReenterCredentials As Boolean
        Private _name As String
        Private _enabled As Boolean

        Public Property Enabled() As Boolean
            Get
                Return _enabled
            End Get
            Set(ByVal value As Boolean)
                _enabled = value
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

		Public Property ThumbUrl() As String

		Public Overrides Function GetHashCode() As Integer
        End Function
    End Class
End Namespace