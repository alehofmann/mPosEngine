Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain
Imports DCS.ProjectBase.Data.NHibernateSessionMgmt

Namespace Dao

    Public Class ConfigDao
        Inherits GenericDao(Of PosSetting, Long)
        Implements IConfigDao
    

        Public Function GetConfigForPos(ByVal posName As String) As IList(Of Core.Domain.PosSetting) Implements Core.DataInterfaces.IConfigDao.GetConfigForPos
            Dim config As IList(Of PosSetting)

            Try
				config = Session.GetNamedQuery("GetSettings").SetString("posName", posName).List(Of Core.Domain.PosSetting)()
			Finally
                'NHibernateSessionManager.Instance.CloseSession()
            End Try

            Return config

        End Function
    End Class
End NameSpace