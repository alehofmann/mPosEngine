Imports DCS.mPosEngine.Data.Infrastructure
Imports NUnit.Framework

Namespace Infrastructure


    <TestFixture()> _
    Public Class TestTransactionExporter
        <Test()>
        Public Sub TestTransactionExportEngine()
            Dim eng As New HttpExternalSystemConnector
            Dim url As String
            Dim params As IList(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))

            url = "http://192.168.1.44:33004/cas/commitPayment"
            params.Add(New KeyValuePair(Of String, String)("id", "11"))
            params.Add(New KeyValuePair(Of String, String)("cardNumber", "300000"))
            eng.TestExportOperation(url, params)

        End Sub

    End Class
End Namespace

