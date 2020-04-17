Imports DCS.ProjectBase.Core

Namespace Domain.ExternalSales
    Public Class ExternalSystem
        Inherits DomainObject(Of Integer)

        Private _url As String
        Private _params As IDictionary(Of String, String)
        Private _name As String

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property Url() As String
            Get
                Return _url
            End Get
            Set(ByVal value As String)
                _url = value
            End Set
        End Property

        Public Property Params As IDictionary(Of String, String)
            Get
                Return _params
            End Get
            Set(value As IDictionary(Of String, String))
                _params = value
            End Set
        End Property

        Public Function GetParsedUrl() As String
            Dim parameter As KeyValuePair(Of String, String)
            Dim retVal As String = _url

            For Each parameter In Params
                retVal = retVal.Replace("%" & parameter.Key & "%", parameter.Value)
            Next

            Return retVal
        End Function
        Public Overrides Function GetHashCode() As Integer

        End Function
    End Class
End Namespace