Imports DCS.mPosEngine.Core.Domain.Sales
Imports DCS.mPosEngine.Core.Domain.Sales.Payment

Namespace DataInterfaces
	Public Interface ITreasuryEngine
		Enum LoginResultCodesEnum
			LoginSuccess = 1
			NoOpenSession = 2
			InvalidCashierId = 3
		End Enum
        Enum CommitResultCodesEnum
            CommitSuccess = 1
            NoActiveSession = 2
        End Enum

        Function CommitToTreasury(ByVal paymentData As PaymentData, ByVal transactionId As Long, ByVal operatorId As Integer, ByVal mPosName As String) As CommitResultCodesEnum
        Function LoginCashier(ByVal cashierId As Long) As LoginResultCodesEnum
        Sub RegisterCardTransfer(ByVal cashierId As Integer, ByVal transferId As Integer)
        sub RegisterVoucher(logVoucherId As Long, voucherType As Integer, voucherAmount As Decimal, posCashierId As Integer, posOperatorId As Integer, posId As String, Optional sqlTrans As SqlClient.SqlTransaction = Nothing)
    End Interface
End Namespace