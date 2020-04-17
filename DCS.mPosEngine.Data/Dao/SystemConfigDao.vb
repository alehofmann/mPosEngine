Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.mPosEngine.Core.Domain
Imports DCS.ProjectBase.Data

Imports NHibernate
Imports NHibernate.Criterion

Namespace Dao
	Public Class SystemConfigDao
		Inherits GenericDao(Of SystemConfig, Integer)
		Implements ISystemConfigDao

		'Public Function FindAll() As System.Collections.Generic.IList(Of Core.Domain.SystemConfig) Implements ProjectBase.Data.IGenericDao(Of Core.Domain.SystemConfig, Integer).FindAll
		'	Dim criteria As ICriteria

		'	'criteria = Session.CreateCriteria(GetType(AdminFunction)).Add(Restrictions.Eq("Enabled", True))
		'	criteria = Session.CreateCriteria(GetType(SystemConfig)).Add(Restrictions.Eq("Program", "POS"))

		'	Return criteria.List(Of SystemConfig)()
		'End Function

	End Class
End Namespace