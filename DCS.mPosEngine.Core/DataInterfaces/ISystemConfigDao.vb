Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain

Namespace DataInterfaces
	Public Interface ISystemConfigDao
		Inherits IGenericDao(Of SystemConfig, Integer)
	End Interface
End Namespace