Imports System.Reflection
Imports System.Configuration.Install

Public Class SelfInstaller
    Private Shared ReadOnly _exePath As String = Assembly.GetExecutingAssembly().Location

    Public Shared Function InstallMe() As Boolean
        Try
            ManagedInstallerClass.InstallHelper(New String() {_exePath})

        Catch
            Return False
        End Try

        Return True
    End Function

    Public Shared Function UninstallMe() As Boolean
        Try
            ManagedInstallerClass.InstallHelper(New String() {"/u", _exePath})
        Catch
            Return False
        End Try

        Return True
    End Function
End Class
