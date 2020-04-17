Imports DCS.mPosEngine.Core
Imports DCS.mPosEngine.Core.Domain.Sales.Payment
Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.TreasuryEngine

Namespace Infrastructure
    Public Class TreasuryEngine
        Implements DCS.mPosEngine.Core.DataInterfaces.ITreasuryEngine

        Private _treasuryEngine As DCS.TreasuryEngine.TreasuryEngine
        Private _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Sub New()
            _treasuryEngine = New DCS.TreasuryEngine.TreasuryEngine()
        End Sub

        Public Function CommitToTreasury(paymentData As Core.Domain.Sales.Payment.PaymentData, transactionId As Long, operatorId As Integer, mPosName As String) As DCS.mPosEngine.Core.DataInterfaces.ITreasuryEngine.CommitResultCodesEnum Implements Core.DataInterfaces.ITreasuryEngine.CommitToTreasury
            Dim mt As New DCS.TreasuryEngine.MoneyTransaction

            For Each payItem As PayItem In paymentData.PayItems
                mt.Add(New DCS.TreasuryEngine.MoneyOperation(payItem.Currency.PaymodeType, payItem.Amount))
            Next

            Try
                _treasuryEngine.RegistermPosPayment(operatorId, transactionId, mt, mPosName)
            Catch ex As ApplicationException
                Return DCS.mPosEngine.Core.DataInterfaces.ITreasuryEngine.CommitResultCodesEnum.NoActiveSession
            End Try

            Return DCS.mPosEngine.Core.DataInterfaces.ITreasuryEngine.CommitResultCodesEnum.CommitSuccess
        End Function

        Public Function LoginCashier(cashierId As Long) As Core.DataInterfaces.ITreasuryEngine.LoginResultCodesEnum Implements Core.DataInterfaces.ITreasuryEngine.LoginCashier
            Dim cashier As Employee
            Dim session As DCS.TreasuryEngine.TreasurySession

            Try
                cashier = New DCS.TreasuryEngine.Employee(cashierId)
            Catch ex As ApplicationException
                Return DCS.mPosEngine.Core.DataInterfaces.ITreasuryEngine.LoginResultCodesEnum.InvalidCashierId
            End Try

            session = _treasuryEngine.GetSession(cashier)

            '   Braian ERROR ARREGLAR ESTO, MOSTRAR MENSAJE SI NO HAY SESION ACTIVA

            If Not session Is Nothing Then
                If session.State = DCS.TreasuryEngine.TreasuryEngine.enuSessionStates.ssOpen Then
                    _treasuryEngine.SessionIsNowActive(session)
                ElseIf session.State <> DCS.TreasuryEngine.TreasuryEngine.enuSessionStates.ssActive Then
                    Return DCS.mPosEngine.Core.DataInterfaces.ITreasuryEngine.LoginResultCodesEnum.NoOpenSession
                End If

                Return DCS.mPosEngine.Core.DataInterfaces.ITreasuryEngine.LoginResultCodesEnum.LoginSuccess
            Else
                Return DCS.mPosEngine.Core.DataInterfaces.ITreasuryEngine.LoginResultCodesEnum.NoOpenSession
            End If
        End Function

        Public Sub RegisterCardTransfer(cashierId As Integer, transferId As Integer) Implements DataInterfaces.ITreasuryEngine.RegisterCardTransfer
            _treasuryEngine.RegisterCardTransfer(cashierId, transferId)
        End Sub

        Private sub RegisterVoucher(logVoucherId As Long, voucherType As Integer, voucherAmount As Decimal, posCashierId As Integer, posOperatorId As Integer, posId As String, Optional transaction As SqlClient.SqlTransaction = Nothing) Implements DataInterfaces.ITreasuryEngine.RegisterVoucher        
            _log.Info("Registering voucher in treasury")
            _log.Debug("logVoucherId: " & logVoucherId)

            Dim vCount As New VoucherCount
            vCount.Add(New VoucherCountItem(voucherType, voucherAmount, 1))

            _treasuryEngine.RegisterVoucherRedemption(posCashierId, posOperatorId, logVoucherId, vCount, posId, transaction)
            _log.Info("Registered ok")

        End sub
    End Class
End Namespace