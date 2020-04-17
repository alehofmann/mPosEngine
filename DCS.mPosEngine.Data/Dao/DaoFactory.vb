
Namespace Dao

    Public MustInherit Class DaoFactory
        Public MustOverride Function GetProductDao() As ProductDao
        Public MustOverride Function GetConfigDao() As ConfigDao
        Public MustOverride Function GetVoucherDao() As VoucherDao
        'Public MustOverride Function GetRedeemVoucherDao() As RedeemVoucherDao
        Public MustOverride Function GetDeviceDao() As DeviceDao
        Public MustOverride Function GetAdminFunctionDao() As AdminFunctionDao
        Public MustOverride Function GetUserDao() As UserDao
        Public MustOverride Function GetMPosTransactionDao() As MPosTransactionDao
        Public MustOverride Function GetProductPageDao() As ProductPageDao
		Public MustOverride Function GetTransactionExporterDao() As TransactionExporterDao
		Public MustOverride Function GetSystemConfigDao() As SystemConfigDao
	End Class

	Public Class NHibernateDaoFactory
		Inherits DaoFactory

        Public Overrides Function GetProductDao() As ProductDao
            Return New ProductDao
        End Function

        Public Overrides Function GetVoucherDao() As VoucherDao
            Return New VoucherDao
        End Function

        'Public Overrides Function GetRedeemVoucherDao() As RedeemVoucherDao
        '    Return New RedeemVoucherDao
        'End Function

        Public Overrides Function GetConfigDao() As ConfigDao
			Return New ConfigDao
		End Function

		Public Overrides Function GetDeviceDao() As DeviceDao
			Return New DeviceDao
		End Function

		Public Overrides Function GetAdminFunctionDao() As AdminFunctionDao
			Return New AdminFunctionDao
		End Function

		Public Overrides Function GetUserDao() As UserDao
			Return New UserDao
		End Function

		Public Overrides Function GetMPosTransactionDao() As MPosTransactionDao
			Return New MPosTransactionDao
		End Function

		Public Overrides Function GetProductPageDao() As ProductPageDao
			Return New ProductPageDao
		End Function

		Public Overrides Function GetTransactionExporterDao() As TransactionExporterDao
			Return New TransactionExporterDao
		End Function

		Public Overrides Function GetSystemConfigDao() As SystemConfigDao
			Return New SystemConfigDao()
		End Function
	End Class
End NameSpace