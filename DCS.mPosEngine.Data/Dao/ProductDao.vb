Imports NHibernate.Criterion
Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain.Sales
Imports NHibernate

Namespace Dao

    Public Class ProductDao
        Inherits GenericDao(Of Product, Long)

        Implements IProductDao


        Public Shadows Function FindAll() As System.Collections.Generic.IList(Of Product) Implements ProjectBase.Data.IGenericDao(Of Product, Long).FindAll
            Dim criteria As ICriteria

            criteria = Session.CreateCriteria(GetType(Product)).Add(Restrictions.Eq("Deleted", False))

            Return criteria.List(Of Product)()

        End Function


    End Class
End NameSpace