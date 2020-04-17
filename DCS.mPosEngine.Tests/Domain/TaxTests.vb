Imports DCS.mPosEngine.Data.Infrastructure
Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment
Imports NUnit.Framework
Imports DCS.mPosEngine.Core.Domain.Sales

Namespace Domain
    Public Class TaxTests

        <Test()>
        Public Sub GetTaxTotal()
            Dim transaction As MPosTransaction = GetSingleOperationTransaction(2, 1, 50)
            Dim transactionTaxInfo As TransactionTaxInfo

            transactionTaxInfo = transaction.GetTotals.TransactionTaxInfo

            Assert.IsNotNull(transactionTaxInfo)
            Assert.IsTrue(transactionTaxInfo.GetSalesTaxTotal.Equals(transaction.GetTotals.Subtotal2 * (New Decimal(0.1))))

        End Sub

        <Test()>
        Public Sub GetTaxItems()
            Dim transaction As MPosTransaction = GetSingleOperationTransaction(2, 1, 50)
            Dim transactionTaxInfo As TransactionTaxInfo
            Dim productDao As DCS.mPosEngine.Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao
            Dim product As Product

            product = productDao.FindByID(2)

            transactionTaxInfo = transaction.GetTotals.TransactionTaxInfo

            Assert.IsNotNull(transactionTaxInfo)
            Assert.IsTrue(transactionTaxInfo.TaxItems.Count = product.ProductData.TaxTypes.Count)
        End Sub
        Private Function GetSingleOperationTransaction(ByVal productId As Integer, ByVal settledPrice As Decimal, ByVal quantity As Decimal) As MPosTransaction
			Dim retVal As New MPosTransaction(15, "VERUGO", 1)
			Dim productDao As DCS.mPosEngine.Data.Dao.ProductDao = (New DCS.mPosEngine.Data.Dao.NHibernateDaoFactory).GetProductDao
            Dim product As Product

            product = productDao.FindByID(productId)

            If product.ProductData.NeedsCard And Not product.ProductData.SellsNewCard Then
                retVal.AddLineitem(settledPrice, quantity, product.ProductData, GetSoldCard)
            ElseIf product.ProductData.SellsNewCard Then
                retVal.AddLineitem(settledPrice, quantity, product.ProductData, GetNotSoldCard)
            Else
                retVal.AddLineitem(settledPrice, quantity, product.ProductData)
            End If



            Return retVal
        End Function

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

    End Class
End Namespace
