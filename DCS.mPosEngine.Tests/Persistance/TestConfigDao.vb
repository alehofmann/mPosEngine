Imports NUnit.Framework
Imports DCS.mPosEngine.Data.Dao
Imports DCS.mPosEngine.Core.DataInterfaces

Namespace Persistance
	<TestFixture()>
	Public Class TestConfigDao
		<Test()>
		Public Sub TestFindAll()
			Dim df As New NHibernateDaoFactory
			Dim dao As ISystemConfigDao = df.GetSystemConfigDao

			Dim list = dao.FindAll()

		End Sub
	End Class
End Namespace