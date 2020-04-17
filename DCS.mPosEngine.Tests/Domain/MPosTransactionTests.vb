Imports DCS.mPosEngine.Data.Infrastructure
Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment
Imports NUnit.Framework
Imports DCS.mPosEngine.Core.Domain.Sales

Namespace Domain

    <TestFixture()> _
    Public Class MPosTransactionTests


        <Test()>
        Public Sub GetTestTransaction_UserCardProduct_NotSoldCard()
            Dim retVal As New MPosTransaction(15, "VERUGO","12334",false)
            Dim productDao As DCS.mPosEngine.Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao
            Dim product As Product

            product = productDao.FindByID(2)

            Try
                retVal.AddLineitem(product.ProductData.Price, 1, product.ProductData, GetNotSoldCard)
                Assert.Fail("Expecting Exception and didn't happened")
            Catch ex As CardNotSoldException
            End Try

        End Sub

        <Test()>
        Public Sub CreditsReturnOperation_NotSoldCard()
            Dim retVal As New MPosTransaction(15, "VERUGO","222")
            Dim productDao As DCS.mPosEngine.Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao
            Dim product As Product

            product = productDao.FindByID(2)

            Try
                retVal.AddLineitem(product.ProductData.Price, -10, product.ProductData, GetNotSoldCard)
                Assert.Fail("Expecting Exception and didn't happened")
            Catch ex As CardNotSoldException
            End Try
        End Sub

        <Test()>
        Public Sub CardReturnOperation_NotSoldCard()
            Dim retVal As New MPosTransaction(15, "VERUGO","333")
            Dim productDao As DCS.mPosEngine.Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao
            Dim product As Product

            product = productDao.FindByID(1)

            Try
                retVal.AddLineitem(product.ProductData.Price, -1, product.ProductData, GetNotSoldCard)
                Assert.Fail("Expecting Exception and didn't happened")
            Catch ex As CardNotSoldException
            End Try
        End Sub


        <Test()>
        Private Function GetTestTransaction_CardSellAndCardRechargeInDifferentOperations()

        End Function
        <Test()>
        Public Sub GetTestTransaction_NewCardProduct_SoldCard()
            Dim retVal As New MPosTransaction(15, "VERUGO","333")
            Dim productDao As DCS.mPosEngine.Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao
            Dim product As Product

            product = productDao.FindByID(1)

            Try
                retVal.AddLineitem(product.ProductData.Price, 1, product.ProductData, GetSoldCard)
                Assert.Fail("Expecting Exception and didn't happened")
            Catch ex As CardAlreadySoldException

            End Try

        End Sub

        <Test()>
        Public Sub AddReturnLineitem()
            Dim ts As New MPosTransaction(15, "VERUGO","333")
            Dim productDao As DCS.mPosEngine.Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao
            Dim product As Product
            Dim operation1 As MPosOperation
            Dim operation2 As MPosOperation

            product = productDao.FindByID(2)
            operation1 = ts.AddLineitem(product.ProductData.Price, -10, product.ProductData, GetSoldCard)

            product = productDao.FindByID(13)
            operation2 = ts.AddLineitem(product.ProductData.Price, -1, product.ProductData)

            Assert.IsTrue(ts.GetTotals.TotalToPay < 0)

            Assert.AreEqual(ts.GetTotals.TotalToPay, Decimal.Round(operation1.TotalToPay + operation2.TotalToPay, 2))
            Assert.AreEqual(ts.GetTotals.TotalSalesTax, operation1.TotalSalesTax + operation2.TotalSalesTax)
            Assert.AreEqual(ts.GetTotals.Subtotal1, operation1.Subtotal + operation2.Subtotal)
            Assert.AreEqual(ts.GetTotals.Subtotal2, operation1.Subtotal2 + operation2.Subtotal2)            

        End Sub

        <Test()>
        Public Sub TransactionTotalsAreCoherentWithOperationTotals()
            Dim ts As New MPosTransaction(15, "VERUGO","333")
            Dim productDao As DCS.mPosEngine.Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao
            Dim product As Product
            Dim operation1 As MPosOperation
            Dim operation2 As MPosOperation

            product = productDao.FindByID(72)
            operation1 = ts.AddLineitem(product.ProductData.Price, 2, product.ProductData, GetSoldCard)

            product = productDao.FindByID(73)
            operation2 = ts.AddLineitem(product.ProductData.Price, 13, product.ProductData)

            Assert.AreEqual(ts.GetTotals.TotalToPay, Decimal.Round(operation1.TotalToPay + operation2.TotalToPay, 2))
            Assert.AreEqual(ts.GetTotals.TotalSalesTax, operation1.TotalSalesTax + operation2.TotalSalesTax)
            Assert.AreEqual(ts.GetTotals.Subtotal1, operation1.Subtotal + operation2.Subtotal)
            Assert.AreEqual(ts.GetTotals.Subtotal2, operation1.Subtotal2 + operation2.Subtotal2)
			'Assert.AreEqual(ts.GetTotals.TotalDiscount, operation1.OperationDiscountAmount + operation2.OperationDiscountAmount)

		End Sub


        Private Function GetSoldCard() As CardInfo
            Dim cardManager As New PlaycardBaseCardManager
            Dim cardNumber As Long = 26955373

            'Set preconditions***************
            If Not cardManager.IsCardSold(cardNumber) Then
                cardManager.SellCard(cardNumber)
            End If
            '********************************

            Return CardManagerFactory.GetCardManager().GetCardInfo(cardNumber)
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

        <Test()>
        Public Sub GetTestTransaction_MoreThanOneNewCardProduct()
            Dim retVal As New MPosTransaction(15, "VERUGO","333")
            Dim productDao As DCS.mPosEngine.Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao
            Dim product As Product

            product = productDao.FindByID(1)

            Try
                retVal.AddLineitem(product.ProductData.Price, 50, product.ProductData, GetNotSoldCard)
                Assert.Fail("Expecting Exception and didn't happened")
            Catch ex As CannotAddMoreThanOneNewCardProductToLineitemException
            End Try

        End Sub

        <Test()>
        Public Sub ReturnMoreThanOneNewCardProduct()
            Dim retVal As New MPosTransaction(15, "VERUGO","333")
            Dim productDao As DCS.mPosEngine.Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao
            Dim product As Product

            product = productDao.FindByID(1)

            Try
                retVal.AddLineitem(product.ProductData.Price, -50, product.ProductData, GetNotSoldCard)
                Assert.Fail("Expecting Exception and didn't happened")
            Catch ex As CannotAddMoreThanOneNewCardProductToLineitemException
            End Try

        End Sub

        <Test()>
        Public Sub GetTestTransaction_NoCardProduct()
            Dim retVal As New MPosTransaction(15, "VERUGO","333")
            Dim productDao As DCS.mPosEngine.Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao
            Dim product As Product

            product = productDao.FindByID(13)

            retVal.AddLineitem(product.ProductData.Price, 3, product.ProductData)

            Assert.IsTrue(retVal.Operations.Count = 1)

        End Sub
        <Test()>
        Public Sub GetTestTransaction_CardSale_And_VariableCharge()
            Dim retVal As New MPosTransaction(15, "VERUGO","333")
            Dim productDao As DCS.mPosEngine.Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao
            Dim product As Product

            product = productDao.FindByID(1)

            retVal.AddLineitem(product.ProductData.Price, 1, product.ProductData, GetNotSoldCard)

            product = productDao.FindByID(2)

            retVal.AddLineitem(product.ProductData.Price, 50, product.ProductData, GetNotSoldCard)

            Assert.IsTrue(retVal.Operations.Count = 2)

        End Sub

        Private Function GetSoldCardWithBalance(ByVal cardNumber As Long, ByVal balance As Decimal) As CardInfo
            Dim cardManager As New PlaycardBaseCardManager            

            'Set preconditions***************
            If cardManager.IsCardSold(cardNumber) Then
                cardManager.WipeCard(cardNumber)
            End If

            cardManager.SellCard(cardNumber)
            '********************************

            cardManager.ChargeCard(26955373, 1, balance)

            Return CardManagerFactory.GetCardManager().GetCardInfo(cardNumber)
        End Function

    End Class
End Namespace
