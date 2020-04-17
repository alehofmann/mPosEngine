Imports DCS.ProjectBase.Core

Namespace Domain
    Public Class PosDevice
        Inherits DomainObject(Of Long)

        Private _serialNumber As String
        Private _name As String
        Private _enabled As Boolean
        Private _pairCode As String
        Private _deleted As Boolean
        Private _computerId As Integer


        Public Property Deleted() As Boolean
            Get
                Return _deleted
            End Get
            Set(ByVal value As Boolean)
                _deleted = value
            End Set
        End Property

        Public Property PairCode() As String
            Get
                Return _pairCode
            End Get
            Set(ByVal value As String)
                _pairCode = value
            End Set
        End Property

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

        Public Property SerialNumber() As String
            Get
                Return _serialNumber
            End Get
            Set(ByVal value As String)
                _serialNumber = value
            End Set
        End Property

        Public Property ComputerId As Integer
            Get
                Return _computerId
            End Get
            Set(value As Integer)
                _computerid = value
            End Set
        End Property
        Public ReadOnly Property IsPaired As Boolean
            Get
                Return (Not SerialNumber Is Nothing)
            End Get
        End Property

        Public Overrides Function GetHashCode() As Integer

        End Function
    End Class
End Namespace
