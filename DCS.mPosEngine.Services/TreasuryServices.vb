Imports System

Imports DCS.mPosEngine.Services.Dto
Imports DCS.TreasuryEngine

Public Class TreasuryServices
    Private Shared _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private _treasuryEngine As DCS.TreasuryEngine.TreasuryEngine
    Private _treasuryEnabled As Boolean

    Public Sub New()
        _log.Debug("Instancing TreasuryEngine")
        _treasuryEngine = New TreasuryEngine.TreasuryEngine()

        _log.Info("Creating Config Engine")
        Dim configEngine As New ConfigEngine.Engine("POSEngine")

        _treasuryEnabled = CBool(configEngine.GetItem("Treasury", "Enabled", 0))
        If _treasuryEnabled Then
            _log.Info("Treasury is enabled, instancing Treasury Engine")
            Try
                _treasuryEngine = New TreasuryEngine.TreasuryEngine()
            Catch ex As Exception
                Throw New ApplicationException("Error Creating Treasury engine", ex)
            End Try
        Else
            _log.Warn("Treasury is disabled")
        End If
    End Sub

    Public Function CashPull(cashierId As Integer, moneyOperations As List(Of MoneyOperationDto)) As Boolean
        If _treasuryEngine Is Nothing Then
            Throw New ApplicationException("Treasury not initialized")
        End If

        Dim cashier As Employee
        Dim session As TreasurySession
        Dim res As Boolean

        If moneyOperations.Count = 0 Then
            Throw New ApplicationException("Can't make cash pull without money transactions")
        End If

        Try
            cashier = New DCS.TreasuryEngine.Employee(cashierId)
        Catch ex As ApplicationException
            _log.Error("Invalid cashier id (" + cashierId.ToString() + ")")
            Throw New ApplicationException("Invalid cashier id (" + cashierId.ToString() + ")")
        End Try

        session = _treasuryEngine.GetSession(cashier)

        If session.State <> DCS.TreasuryEngine.TreasuryEngine.enuSessionStates.ssActive Then
            session.IsNowActive()
        End If

        If session.CanCashPull Then
            Dim moneyTransaction As New MoneyTransaction

            moneyOperations.ForEach(Sub(x) moneyTransaction.Operations.Add(New MoneyOperation(x.CurrencyId, x.Amount)))

            _treasuryEngine.CashPull(cashier, cashierId, moneyTransaction)

            res = True
        Else
            _log.Debug("Cashier id " + cashierId.ToString() + " is not allowed make cash pull")
        End If

        Return res
    End Function

    Public Function PayOut(cashierId As Integer, reason As String, moneyOperations As List(Of MoneyOperationDto)) As Boolean
        If _treasuryEngine Is Nothing Then
            Throw New ApplicationException("Treasury not initialized")
        End If

        If reason Is Nothing Then reason = String.Empty

        Dim cashier As Employee
        Dim session As DCS.TreasuryEngine.TreasurySession
        Dim res As Boolean

        If moneyOperations.Count = 0 Then
            Throw New ApplicationException("Can't make pay out without money transactions")
        End If

        Try
            cashier = New DCS.TreasuryEngine.Employee(cashierId)
        Catch ex As ApplicationException
            _log.Error("Invalid cashier id (" + cashierId.ToString() + ")")
            Throw New ApplicationException("Invalid cashier id (" + cashierId.ToString() + ")")
        End Try

        session = _treasuryEngine.GetSession(cashier)

        If session.State <> DCS.TreasuryEngine.TreasuryEngine.enuSessionStates.ssActive Then
            session.IsNowActive()
        End If

        If session.CanPayout Then
            Dim moneyTransaction As New MoneyTransaction

            moneyOperations.ForEach(Sub(x) moneyTransaction.Operations.Add(New MoneyOperation(x.CurrencyId, x.Amount)))

            _treasuryEngine.Payout(cashier, cashierId, moneyTransaction, reason)

            res = True
        Else
            _log.Debug("Cashier id " + cashierId.ToString() + " is not allowed make pay out")
        End If

        Return res
    End Function

    Public Function GetBalance(cashierId As Integer) As TreasuryBalanceDto
        If _treasuryEngine Is Nothing Then
            Throw New ApplicationException("Treasury not initialized")
        End If

        Dim cashier As Employee
        Dim session As DCS.TreasuryEngine.TreasurySession

        Try
            cashier = New DCS.TreasuryEngine.Employee(cashierId)
        Catch ex As ApplicationException
            _log.Error("Invalid cashier id (" + cashierId.ToString() + ")")
            Throw New ApplicationException("Invalid cashier id (" + cashierId.ToString() + ")")
        End Try

        session = _treasuryEngine.GetSession(cashier)

        If session.State <> DCS.TreasuryEngine.TreasuryEngine.enuSessionStates.ssActive Then
            session.IsNowActive()
        End If

        Dim dBalance = session.Balance(True)

        Dim c As New Currencies()

        Return DrawerBalanceToBalanceDto(dBalance, c)
    End Function

    Private Function DrawerBalanceToBalanceDto(db As DrawerBalance, currencies As Currencies) As TreasuryBalanceDto
        Dim res As New TreasuryBalanceDto

        For i As Integer = 1 To db.MoneyBalance.Operations.Count
            If db.MoneyBalance.Operations(i) IsNot Nothing Then
                Dim currency = currencies(db.MoneyBalance.Operations(i).CurrencyID)

                If currency Is Nothing Then
                    '	Esto no debería pasar
                    _log.Error("Currency Id (" + db.MoneyBalance.Operations(i).CurrencyID.ToString() + ") is not recognized")
                    Throw New ApplicationException("Currency Id (" + db.MoneyBalance.Operations(i).CurrencyID.ToString() + ") is not recognized")
                End If

                res.AddCash(db.MoneyBalance.Operations(i).CurrencyID, currency.Name, db.MoneyBalance.Operations(i).Amount)
            End If
        Next

        For i As Integer = 1 To db.VoucherBalance.Items.Count
            If db.VoucherBalance.Items(i) IsNot Nothing Then
                res.AddVoucher(db.VoucherBalance.Items(i).VoucherTypeId, db.VoucherBalance.Items(i).CountAmount, db.VoucherBalance.Items(i).CountQuantity)
            End If
        Next

        Return res
    End Function
End Class