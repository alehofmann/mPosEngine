Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain

Namespace DataInterfaces
    Public Interface IAdminFunctionDao
        Inherits IGenericDao(Of AdminFunction, Integer)
    End Interface
End Namespace