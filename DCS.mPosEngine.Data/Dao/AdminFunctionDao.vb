Imports NHibernate
Imports NHibernate.Criterion
Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain

Namespace Dao
    Public Class AdminFunctionDao
        Inherits GenericDao(Of AdminFunction, Integer)
        Implements IAdminFunctionDao


        Public Function FindAll() As System.Collections.Generic.IList(Of Core.Domain.AdminFunction) Implements ProjectBase.Data.IGenericDao(Of Core.Domain.AdminFunction, Integer).FindAll            
            Dim criteria As ICriteria

            criteria = Session.CreateCriteria(GetType(AdminFunction)).Add(Restrictions.Eq("Enabled", True))

            Return criteria.List(Of AdminFunction)()
        End Function

        
    End Class
End NameSpace