Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.mPosEngine.Core.Domain.Sales
Imports DCS.mPosEngine.Core.Domain.Sales.Payment
Imports DCS.mPosEngine.Data.Dao
Imports NUnit.Framework

Namespace Persistance

	

	<TestFixture()> _
	Public Class TestDiscounts
		
		private _operatorId As Integer=1
		private _posName As String="VERUGO"

		<Test()>
		Public sub TestAddDiscountToOperation
			Dim df As New NHibernateDaoFactory
			Dim dao As IMPosTransactionDao = df.GetMPosTransactionDao

			Dim transaction As MPosTransaction

			transaction=dao.GetById(1)

			transaction.Operations.First.AddDiscount(1, 1)

			dao.MakePersistent(transaction)
			dao.Session.flush

		End sub
	End Class

End Namespace