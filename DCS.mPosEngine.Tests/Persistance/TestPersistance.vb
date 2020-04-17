Imports DCS.mPosEngine.Data.Dao
Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.mPosEngine.Data
Imports DCS.mPosEngine.Core.Domain
Imports NHibernate.Tool.hbm2ddl
Imports NUnit.Framework
Imports NHibernate.Cfg
Imports DCS.mPosEngine.Core.Domain.Sales
Imports DCS.mPosEngine.Core.Domain.Pagesets

Namespace Persistance
    <TestFixture()> _
    Public Class TestPersistance
        <Test()> _
        Public Sub GenerateSchema()
            Dim cfg As New Configuration

            log4net.Config.XmlConfigurator.Configure()
            cfg.Configure()
            Dim sch As New SchemaExport(cfg)

            sch.Execute(True, True, False)
        End Sub

        <Test()> _
        Public Sub GetProducts()
            Dim df As New NHibernateDaoFactory
            Dim dao As iproductdao = df.GetProductDao

            Dim products As IList(Of product)

            products = dao.FindAll

        End Sub

        <Test()> _
        Public Sub GetAdminFunctions()
            Dim df As New NHibernateDaoFactory
            Dim dao As IAdminFunctionDao = df.GetAdminFunctionDao

            Dim af As IList(Of AdminFunction)

            af = dao.FindAll

        End Sub

        <Test()> _
        Public Sub GetPosDevice()
            Dim df As New NHibernateDaoFactory
            Dim dao As IDeviceDao = df.GetDeviceDao
            Dim posDevice As PosDevice

            posDevice = dao.GetDeviceByName("VERUGO")

        End Sub
        <Test()> _
        Public Sub GetConfig()
            Dim df As New NHibernateDaoFactory
            Dim dao As IConfigDao = df.GetConfigDao

            Dim settings As IList(Of PosSetting)

            settings = dao.GetConfigForPos(1)

        End Sub

        <Test()> _
        Public Sub GetUsers()
            Dim df As New NHibernateDaoFactory
            Dim dao As IUserDao = df.getuserdao

            Dim users As IList(Of User)

            users = dao.FindAll
        End Sub

        
        <Test()> _
        Public Sub GetProductPages()
            log4net.Config.XmlConfigurator.Configure()

            Dim df As New NHibernateDaoFactory
            Dim dao As IProductPageDao = df.GetProductPageDao

            Dim pages As IList(Of ProductPage)

            pages = dao.FindAll
        End Sub

        <Test()> _
        Public Sub GetPagesForPos()
            log4net.Config.XmlConfigurator.Configure()

            Dim df As New NHibernateDaoFactory
            Dim dao As IProductPageDao = df.GetProductPageDao

            Dim pages As IList(Of ProductPage)

            pages = dao.GetPagesForPos("VERUGO")
        End Sub

        <Test()> _
        Public Sub GetUseByCardNumebr()
            Dim df As New NHibernateDaoFactory
            Dim dao As IUserDao = df.GetUserDao

            Dim user As User

            user = dao.GetByCardNumber(6621952)

        End Sub
    End Class
End Namespace