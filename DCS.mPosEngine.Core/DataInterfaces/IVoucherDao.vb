Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain.Sales

Namespace DataInterfaces
    Public Interface IVoucherDao
        Inherits IGenericDao(Of Voucher, Long)

        Function GetVoucherByIdAndType(voucherId As Long, type As Integer) As Voucher
        function RedeemVoucher(voucher as voucher, type as integer,cashierId As Integer) as long

    End Interface

    'Public Interface IRedeemVoucherDao
    '    Inherits IGenericDao(Of RedeemVoucher, Long)

    '    Function RedeemVoucher(type As Integer, id As Long, amount As Decimal, cashierId As Integer) As Long
    'End Interface
End Namespace