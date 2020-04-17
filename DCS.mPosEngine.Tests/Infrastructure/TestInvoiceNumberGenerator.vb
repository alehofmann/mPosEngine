Imports DCS.mPosEngine.Data.Infrastructure
Imports NUnit.Framework

Namespace Infrastructure

    <TestFixture()> _
    Public Class TestInvoiceNumberGenerator

        <Test()>
        Public Sub TestGetInvoiceNumber()
            Dim ing As New InvoiceNumberGenerator
            Dim invoiceNumber1 As String
            Dim invoiceNumber2 As String

            invoiceNumber1 = ing.GetNextAndIncrement
            invoiceNumber2 = ing.GetNextAndIncrement

            Assert.IsTrue(Int(invoiceNumber2) = Int(invoiceNumber1) + 1)
        End Sub
    End Class
End Namespace
