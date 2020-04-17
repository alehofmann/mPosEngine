Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.mPosEngine.Core.Domain.Sales
Imports DCS.ProjectBase.Data

Namespace Dao
    Public Class VoucherDao
        Inherits GenericDao(Of Voucher, Long)
        Implements IVoucherDao

        Private _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Function GetVoucherByIdAndType(voucherId As Long, type As Integer) As Voucher Implements IVoucherDao.GetVoucherByIdAndType
            Dim voucher As Voucher

            'Try
                voucher = Session.GetNamedQuery("GetVoucherByIdAndType").SetInt64("VoucherId", voucherId).SetInt32("TypeId", type).UniqueResult
            'Catch ex As Exception
            '    _log.Error("Can't get voucher info. Message: " + ex.Message, ex)
            'End Try

            Return voucher
        End Function

        Private Function IVoucherDao_RedeemVoucher(voucher As Voucher, type As Integer, cashierId As Integer) As Long Implements IVoucherDao.RedeemVoucher
            'Dim redVoucherId As Long

            'Try
            '    redVoucherId = Session.GetNamedQuery("spRedeemVoucher").SetInt32("Type", type).SetInt64("Id", voucher.id).SetDecimal("Amount", voucher.amount).SetInt32("CashierId", cashierId).UniqueResult
            'Catch ex As Exception
            '    _log.Error("Can't redeem voucher. Message: " + ex.Message, ex)
            '    redVoucherId = -1
            'End Try

            'Return redVoucherId
            return Session.GetNamedQuery("spRedeemVoucher").SetInt32("Type", type).SetInt64("Id", voucher.id).SetDecimal("Amount", voucher.amount).SetInt32("CashierId", cashierId).UniqueResult

        End function

        
    End Class

    'Public Class RedeemVoucherDao
    '    Inherits GenericDao(Of RedeemVoucher, Long)
    '    Implements IRedeemVoucherDao

    '    Private _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    '    Public Function RedeemVoucher(type As Integer, id As Long, amount As Decimal, cashierId As Integer) As Long Implements IRedeemVoucherDao.RedeemVoucher
    '        Dim redVoucherId As Long

    '        Try
    '            redVoucherId = Session.GetNamedQuery("spRedeemVoucher").SetInt32("Type", type).SetInt64("Id", id).SetDecimal("Amount", amount).SetInt32("CashierId", cashierId).UniqueResult
    '        Catch ex As Exception
    '            _log.Error("Can't redeem voucher. Message: " + ex.Message, ex)
    '            redVoucherId = -1
    '        End Try

    '        Return redVoucherId
    '    End Function
    'End Class
End Namespace