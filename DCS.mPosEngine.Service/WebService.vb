Imports System.ServiceModel
Imports System.Runtime.Remoting.Messaging

Public Class WebService    

    Private _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private _serviceHost As ServiceHost
    Private Delegate Sub StartSystemDelegate()

    Public Sub AsyncStartAgent()
        Dim dg As New StartSystemDelegate(AddressOf StartAgent)
        Dim ar As IAsyncResult
        ar = dg.BeginInvoke(New AsyncCallback(AddressOf AgentStarted), Nothing)
    End Sub

    Public Function StartAgent() As Boolean
        'Dim baseAddress As New Uri("net.tcp://192.168.1.118:2582")
        Dim baseAddress As New Uri("http://192.168.1.55:8000")


        Try
            _log.Info("Instancing Host, URI is: " & baseAddress.ToString)
            _serviceHost = New ServiceHost(GetType(WebService), baseAddress)
            _log.Info("Host Instanced OK")
        Catch e As Exception
            _log.Error("Error Instancing Host", e)
            Return False
        End Try

        _log.Info("Opening Host")
        Try
            _serviceHost.Open()
        Catch ex As Exception
            _log.Error("Error Opening Host", ex)
            Return False
        End Try
    End Function


    Private Sub AgentStarted(ByVal ar As System.IAsyncResult)
        CType(CType(ar, AsyncResult).AsyncDelegate, StartSystemDelegate).EndInvoke(ar)
    End Sub
End Class
