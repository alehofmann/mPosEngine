Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json
Imports System.IO

Namespace CommObjects
    <DataContract()>
    Public Class CommandResponse(Of T)
        Private _header As New ResponseHeader
        Private _body As T

        <DataMember(Name:="body")> _
        Public Property Body() As T
            Get
                Return _body
            End Get
            Set(ByVal value As T)
                _body = value
            End Set
        End Property

        <DataMember(Name:="header")> _
        Public Property Header() As ResponseHeader
            Get
                Return _header
            End Get
            Set(ByVal value As ResponseHeader)
                _header = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Dim js As New DataContractJsonSerializer(GetType(CommandResponse(Of T)))
            Dim ms As New MemoryStream

            js.WriteObject(ms, Me)

            ms.Position = 0
            Return (New StreamReader(ms)).ReadToEnd
        End Function
    End Class
End Namespace