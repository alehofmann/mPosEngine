Imports DCS.ProjectBase.Core

Namespace Domain

    Public Class User
        Inherits DomainObject(Of Integer)

        Private _handle As String
        Private _firstName As String
        Private _lastName As String
        Private _password As String
        Private _cardNumber As String
        Private _deleted As Boolean

        Public Property CardNumber() As Long
            Get
                Return IIf(IsNumeric(_cardNumber), _cardNumber, -1)
            End Get
            Set(ByVal value As Long)
                _cardNumber = Str(value)
            End Set
        End Property
        Public Property Deleted As Boolean
            Get
                Return _deleted
            End Get
            Set(value As Boolean)
                _deleted = value
            End Set
        End Property
        Public Property Password As String
            Get
                Return _password
            End Get
            Set(value As String)
                _password = value
            End Set
        End Property
        Public Property Handle As String
            Get
                Return _handle
            End Get
            Set(value As String)
                _handle = value
            End Set
        End Property

        Public Property LastName As String
            Get
                Return (IIf(_LastName Is Nothing, "", _LastName))
            End Get
            Set(value As String)
                _LastName = value
            End Set
        End Property

        Public Property FirstName As String
            Get
                Return (IIf(_firstName Is Nothing, "", _firstName))
            End Get
            Set(value As String)
                _firstName = value
            End Set
        End Property


        Public Overrides Function GetHashCode() As Integer

        End Function
    End Class
End Namespace
