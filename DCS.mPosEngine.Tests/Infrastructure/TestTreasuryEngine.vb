Imports DCS.PlaycardBase.Data
Imports DCS.mPosEngine.Data.Infrastructure
Imports NUnit.Framework
Imports DCS.mPosEngine.Core.Domain.Sales.Payment

Namespace Infrastructure

    <TestFixture()> _
    Public Class TestTreasuryEngine
        <Test()>
        Public Sub TestSuccessfullCommitToTreasury()
            Dim te As New TreasuryEngine
            Dim paymentData As PaymentData = GetPaymentDataForAmount(20)

            Assert.IsTrue(te.CommitToTreasury(paymentData, 1, 7, "VERUGO") = Core.DataInterfaces.ITreasuryEngine.CommitResultCodesEnum.CommitSuccess)
        End Sub

        Private Function GetPaymentDataForAmount(ByVal amount As Decimal) As PaymentData
            Dim currencyDao As DCS.PlaycardBase.Core.DataInterfaces.ICurrencyDao = New currencydao

            Return New PaymentData(New PayItem(currencyDao.FindByID(1), amount, New CashPaymodeData()))
        End Function

    End Class


End Namespace
