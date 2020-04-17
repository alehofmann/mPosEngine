Public Class ConfigItem
    Private _key As String
    Public Property NewProperty() As String
        Get
            Return _key
        End Get
        Set(ByVal value As String)
            _key = value
        End Set
    End Property

End Class
