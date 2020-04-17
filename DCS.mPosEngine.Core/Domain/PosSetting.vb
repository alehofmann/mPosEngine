Imports DCS.ProjectBase.Core

Namespace Domain
    Public Class PosSetting
        Inherits DomainObject(Of Long)

        Private _section As String
        Private _key As String
        Private _settingValue As String

        Public Property SettingValue() As String
            Get
                Return _settingValue
            End Get
            Set(ByVal value As String)
                _settingValue = value
            End Set
        End Property

        Public Property Key() As String
            Get
                Return _key
            End Get
            Set(ByVal value As String)
                _key = value
            End Set
        End Property

        Public Property Section() As String
            Get
                Return _section
            End Get
            Set(ByVal value As String)
                _section = value
            End Set
        End Property


        Public Overrides Function GetHashCode() As Integer

        End Function
    End Class
End Namespace
