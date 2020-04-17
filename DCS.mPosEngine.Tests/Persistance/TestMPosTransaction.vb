Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.mPosEngine.Data.Dao
Imports DCS.mPosEngine.Core.Domain.Sales
Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment
Imports DCS.mPosEngine.Data.Infrastructure
Imports DCS.ProjectBase.Data.NHibernateSessionMgmt
Imports DCS.PlaycardBase.Core.PosDomain

Imports NUnit.Framework
Imports DCS.mPosEngine.Core.Domain.Sales.Payment

Namespace Persistance

    <TestFixture()> _
    Public Class TestMPosTransaction

        <Test()>
        Public Sub TestPersistTransaction()
            Dim df As New NHibernateDaoFactory
            Dim dao As IMPosTransactionDao = df.GetMPosTransactionDao
            Dim persistedId As Integer
            'Dim expectedTransaction As MPosTransaction = GetTestTransaction_CardSale_And_VariableCharge()
            Dim expectedTransaction As MPosTransaction = GetTestTransaction_ProductId71()
            Dim actualTransaction As MPosTransaction

            Using unitofwork As IUnitOfWork = DCS.ProjectBase.Data.NHibernateSessionMgmt.UnitOfWork.Start
                dao.MakePersistent(expectedTransaction)
                unitofwork.Commit()
            End Using

            'dao.Session.Flush()

            persistedId = expectedTransaction.Id

            actualTransaction = dao.FindById(persistedId)
            Assert.IsNotNull(actualTransaction)
            Assert.AreEqual(expectedTransaction.Operations.Count, actualTransaction.Operations.Count)
        End Sub

        Private Function GetTestTransaction_ProductId71() As MPosTransaction
            Dim retVal As New MPosTransaction(15, "VERUGO","333")
            Dim productDao As Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao
            Dim product As Core.Domain.Sales.Product
            Dim currency As Currency
            Dim currencyDao As PlaycardBase.Core.DataInterfaces.ICurrencyDao


            product = productDao.FindByID(71)

            retVal.AddLineitem(10, 8, product.ProductData, CardManagerFactory.GetCardManager().GetCardInfo(3))

            Assert.IsTrue(retVal.Operations.Count = 1)

            'Payment-------------
            currencyDao = (New PlaycardBase.Data.NHibernateDaoFactory).GetCurrencyDao
            currency = currencyDao.FindById(1)
            Dim paymentData As New Payment.PaymentData(New PayItem(currency, 20, New CashPaymodeData))

            currency = currencyDao.FindById(2)
            paymentData.AddPayitem(currency, 30, New CreditCardData("1234", "MCRD", "4321", "Everything Fine!"))

            currency = currencyDao.FindById(3)
            paymentData.AddPayitem(currency, retVal.GetTotals.TotalToPay - 50, New PlaycardData(12435789))

            retVal.Pay(paymentData)
            '*********************

            Return retVal
        End Function
        Private Function GetTestTransaction_CardSale_And_VariableCharge() As MPosTransaction
            Dim retVal As New MPosTransaction(15, "VERUGO","333")
            Dim productDao As Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao
            Dim product As Core.Domain.Sales.Product
            Dim currency As Currency
            Dim currencyDao As PlaycardBase.Core.DataInterfaces.ICurrencyDao


            
            product = productDao.FindByID(1)

            retVal.AddLineitem(product.ProductData.Price, 1, product.ProductData, GetNotSoldCard)

            product = productDao.FindByID(2)

            retVal.AddLineitem(product.ProductData.Price, 50, product.ProductData, GetNotSoldCard)


            Assert.IsTrue(retVal.Operations.Count = 3)

            'Payment-------------
            currencyDao = (New PlaycardBase.Data.NHibernateDaoFactory).GetCurrencyDao
            currency = currencyDao.FindById(1)
            Dim paymentData As New Payment.PaymentData(New PayItem(currency, 20, New CashPaymodeData))

            currency = currencyDao.FindById(2)
            paymentData.AddPayitem(currency, 30, New CreditCardData("1234", "MCRD", "4321", "Everything Fine!"))

            currency = currencyDao.FindById(3)
            paymentData.AddPayitem(currency, 6.1, New PlaycardData(12435789))

            retVal.Pay(paymentData)
            '*********************

            Return retVal
        End Function

        Private Function GetNotSoldCard() As CardInfo
            Dim retVal As CardInfo
            retVal = CardManagerFactory.GetCardManager().GetCardInfo(400000)

            If retVal.IsSold Then
                CardManagerFactory.GetCardManager().WipeCard(400000)
            End If

            retVal = CardManagerFactory.GetCardManager().GetCardInfo(400000)

            Return retVal
        End Function
    End Class
End Namespace
