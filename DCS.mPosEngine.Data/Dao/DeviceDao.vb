Imports NHibernate.Criterion
Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain
Imports NHibernate

Namespace Dao

    Public Class DeviceDao
        Inherits GenericDao(Of PosDevice, Long)
        Implements IDeviceDao




        Public Function GetDeviceByName(name As String) As Core.Domain.PosDevice Implements Core.DataInterfaces.IDeviceDao.GetDeviceByName
            Dim criteria As ICriteria

            Try
                criteria = Session.CreateCriteria(GetType(PosDevice)).Add(Restrictions.Eq("Name", name)).Add(Restrictions.Eq("Deleted", False))
                Return criteria.UniqueResult
            Finally
                'NHibernateSessionManager.Instance.CloseSession()
            End Try


        End Function


        Public Function GetDeviceBySerial(serial As String) As Core.Domain.PosDevice Implements Core.DataInterfaces.IDeviceDao.GetDeviceBySerial

            Dim criteria As ICriteria

            Try
                criteria = Session.CreateCriteria(GetType(PosDevice)).Add(Restrictions.Eq("SerialNumber", serial)).Add(Restrictions.Eq("Deleted", False))
                Return criteria.UniqueResult
            Finally
                'NHibernateSessionManager.Instance.CloseSession()
            End Try



        End Function


        Public Function GetDeviceByPairCode(pairCode As String) As Core.Domain.PosDevice Implements Core.DataInterfaces.IDeviceDao.GetDeviceByPairCode
            Dim criteria As ICriteria

            criteria = Session.CreateCriteria(GetType(PosDevice)).Add(Restrictions.Eq("PairCode", pairCode)).Add(Restrictions.Eq("Deleted", False))
            Return criteria.UniqueResult
        End Function

    End Class
End NameSpace