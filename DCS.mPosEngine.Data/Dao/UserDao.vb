Imports NHibernate.Criterion
Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain
Imports NHibernate

Namespace Dao

    Public Class UserDao
        Inherits GenericDao(Of User, Integer)

        Implements IUserDao


        Public Function GetByCardNumber(cardNumber As Long) As Core.Domain.User Implements Core.DataInterfaces.IUserDao.GetByCardNumber
            Dim criteria As ICriteria

            criteria = Session.CreateCriteria(GetType(User)).Add(Restrictions.Eq("Deleted", False)).Add(Restrictions.Eq("CardNumber", CStr(cardNumber)))
            Return criteria.UniqueResult(Of User)()
        End Function
    End Class
End NameSpace