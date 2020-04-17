Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain
Imports DCS.ProjectBase.Data.NHibernateSessionMgmt
Imports DCS.mPosEngine.Core.Domain.Pagesets
Imports NHibernate

Namespace Dao

    Public Class ProductPageDao
        Inherits GenericDao(Of ProductPage, Integer)
        Implements IProductPageDao


        Public Function GetPagesForPos(posName As String) As IList(Of ProductPage) Implements Core.DataInterfaces.IProductPageDao.GetPagesForPos
            Dim query As IQuery

            query = Session.GetNamedQuery("GetPagesForPos")
            query.SetString("posName", posName)

            Return query.List(Of ProductPage)()
        End Function

        
    End Class
End NameSpace