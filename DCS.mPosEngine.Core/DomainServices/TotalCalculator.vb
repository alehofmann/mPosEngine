Imports DCS.mPosEngine.Core.Domain
Imports DCS.mPosEngine.Core.Domain.Sales

Namespace DomainServices
    Public Class TotalCalculator
        'Public Function GetTransactionTotals(ByVal MPosTransaction As MPosTransaction) As TransactionTotals
        '    Dim MPosOperation As MPosOperation
        '    Dim retVal As New TransactionTotals

        '    For Each MPosOperation In MPosTransaction.Operations
        '        retVal.Subtotal1 += MPosOperation.PriceSettled * MPosOperation.Quantity
        '    Next

        '    If Not MPosTransaction.TaxExempt Then
        '        retVal.TaxInfo = GetTransactionTaxInfo(MPosTransaction)
        '    End If

        '    Return retVal
        'End Function

        'Private Function GetTransactionTaxInfo(ByVal MPosTransaction As MPosTransaction) As TaxInfo
        '    Dim retVal As New TaxInfo
        '    Dim MPosOperation As MPosOperation



        '    For Each MPosOperation In MPosTransaction.Operations
        '        If MPosOperation.Product.ProductData.PaysTax Then                    
        '            'retVal.AddTaxItem(New TransactionTaxInfoItem(1, "Tax de mentira 10%", MPosOperation.Product.ProductData.Price * 0.1))
        '            retVal.AddTax(1, "Tax de mentira 10%", MPosOperation.Product.ProductData.Price * 0.1)
        '            'retVal.AddTaxItem(New TransactionTaxInfoItem(2, "Tax de mentira 20%", MPosOperation.Product.ProductData.Price * 0.2))
        '            retVal.AddTax(2, "Tax de mentira 20%", MPosOperation.Product.ProductData.Price * 0.2)
        '        End If

        '    Next

        '    Return retVal
        'End Function
    End Class
End Namespace
