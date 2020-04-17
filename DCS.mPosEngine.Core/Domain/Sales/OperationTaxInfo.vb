Imports DCS.ProjectBase.Core
Imports DCS.PlaycardBase.Core.PosDomain

Namespace Domain.Sales
    Public Class OperationTaxInfo
        Inherits DomainObject(Of Long)

        Private _taxItems As ISet(Of OperationTaxInfoItem)
        Private readonly _taxTypes As ISet(Of TaxType)
        Private readonly _operation As MPosOperation
        Private readonly _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

		Public Sub New()
		End Sub

		Public Sub New(operation As MPosOperation, taxTypes As ISet(Of TaxType))
			_operation = operation
			_taxTypes = taxTypes

			_log.Debug("Recalculating tax")
			RecalculateTax()
		End Sub

		Public Property TaxItems() As ISet(Of OperationTaxInfoItem)
			Set(value As ISet(Of OperationTaxInfoItem))
				_taxItems = value
			End Set
			Get
				Return _taxItems
			End Get
		End Property

		Public Sub RecalculateTax()
			_taxItems = New HashSet(Of OperationTaxInfoItem)

			Dim baseToUse As Decimal
			Dim totalSoFar As Decimal = baseToUse

			Dim thisTaxTotal As Decimal

			For Each item As TaxType In _taxTypes
				_log.Debug("Adding tax [" & item.Description & "]")

				If item.IsNotAccumulative Then
					baseToUse = _operation.GetTaxBase
				Else
					baseToUse = totalSoFar
				End If

				_log.Debug("--> Tax base is " & baseToUse)
				_log.Debug("--> Tax rate is " & item.TaxRate)
				If item.TaxMode = TaxType.TaxModeEnum.SalesTaxMode Then
					thisTaxTotal = baseToUse * item.TaxRate / 100
					_log.Debug("--> Tax type is 'sales tax'")
				Else
					thisTaxTotal = baseToUse - (baseToUse / (1 + (item.TaxRate / 100)))
					_log.Debug("--> Tax type is 'VAT'")
				End If

				thisTaxTotal = Decimal.Round(thisTaxTotal, Configuration.GetInteger("decimalPlaces", 2))
				_log.Debug("--> Total for this tax is: " & thisTaxTotal & ". Creating 'OperationTaxInfoItem'")
				'Esta pasando 2 veces por aca!!!!!! 
				_taxItems.Add(New OperationTaxInfoItem(_operation, item.TaxMode, item.Id, item.Description, thisTaxTotal))

			Next
		End Sub

		Public Function GetVatTotal() As Decimal
			Dim taxItem As OperationTaxInfoItem
			Dim retVal As Decimal = 0

			For Each taxItem In TaxItems
				If taxItem.TaxSubtype = OperationTaxInfoItem.TaxInfoSubtypeEnum.VatSubtype Then
					retVal += taxItem.TaxAmount
				End If
			Next

			Return retVal
		End Function

		Public Function GetSalesTaxTotal() As Decimal
			Dim taxItem As OperationTaxInfoItem
			Dim retVal As Decimal = 0

			For Each taxItem In TaxItems
				If taxItem.TaxSubtype = OperationTaxInfoItem.TaxInfoSubtypeEnum.SalesTaxSubtype Then
					retVal += taxItem.TaxAmount
				End If
			Next

			Return retVal
		End Function

		Public Overrides Function GetHashCode() As Integer

        End Function
    End Class
End Namespace
