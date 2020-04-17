Imports DCS.mPosEngine.Core.Domain.Shared

Namespace Domain.Sales
	Public Class TransactionTaxInfo
		Implements IValueObject(Of TransactionTaxInfo)

		Private _taxItems As IList(Of TransactionTaxInfoItem) = New List(Of TransactionTaxInfoItem)
		Public Sub New()
		End Sub
		'Public Sub New(ByVal baseAmount As Decimal, ByVal taxTypes As ISet(Of TaxType))
		'    Dim totalSoFar As Decimal = baseAmount
		'    Dim baseToUse As Decimal
		'    Dim thisTaxTotal As Decimal

		'    If taxTypes Is Nothing Then
		'        Throw New ArgumentNullException("taxTypes")
		'    End If

		'    For Each item As TaxType In taxTypes
		'        If item.IsNotAccumulative Then
		'            baseToUse = baseAmount
		'        Else
		'            baseToUse = totalSoFar
		'        End If

		'        If item.TaxMode = TaxType.TaxModeEnum.SalesTaxMode Then
		'            thisTaxTotal = baseToUse * item.TaxRate / 100
		'        Else
		'            thisTaxTotal = baseAmount - (baseAmount / (1 + (item.TaxRate / 100)))
		'        End If

		'        AddTax(item.TaxMode, item.Id, item.Description, thisTaxTotal)
		'    Next
		'End Sub

		Public Property TaxItems() As IList(Of TransactionTaxInfoItem)
			Get
				Return _taxItems
			End Get
			Set(ByVal value As IList(Of TransactionTaxInfoItem))
				_taxItems = value
			End Set
		End Property

		Public Sub AddOperationTaxInfo(ByVal taxInfo As OperationTaxInfo)
			If taxInfo Is Nothing Then Exit Sub

			For Each item As OperationTaxInfoItem In taxInfo.TaxItems
				AddTax(item.TaxSubtype, item.TaxTypeId, item.TaxName, item.TaxAmount)
			Next
		End Sub

		Public Sub AddTaxItem(ByVal transactionTaxInfoItem As TransactionTaxInfoItem)
			If transactionTaxInfoItem Is Nothing Then
				Throw New ArgumentNullException("transactionTaxInfoItem")
			End If

			If Me.Contains(transactionTaxInfoItem.TaxTypeId) Then
				Throw New ArgumentException("Taxinfo already contains tax type id " & transactionTaxInfoItem.TaxTypeId, "transactionTaxInfoItem")
			Else
				_taxItems.Add(transactionTaxInfoItem)
			End If
		End Sub

		Public Sub AddTax(ByVal subtype As TransactionTaxInfoItem.TaxInfoSubtypeEnum, ByVal taxTypeId As Integer, ByVal taxName As String, ByVal taxAmount As Decimal)
			Dim index As Integer

			If Not Me.Contains(taxTypeId, index) Then
				_taxItems.Add(New TransactionTaxInfoItem(subtype, taxTypeId, taxName, taxAmount))
			Else
				_taxItems.Item(index).TaxAmount += taxAmount
			End If
		End Sub

		Public Function Contains(ByVal taxTypeId As Integer, Optional ByRef index As Integer = -1) As Boolean
			Dim item As TransactionTaxInfoItem
			index = -1

			For Each item In _taxItems
				index += 1
				If item.TaxTypeId = taxTypeId Then
					Return True
				End If
			Next

			index = -1
			Return False
		End Function

		Public Function GetVatTotal() As Decimal
			Dim transactionTaxItem As TransactionTaxInfoItem
			Dim retVal As Decimal = 0

			For Each transactionTaxItem In _taxItems
				If transactionTaxItem.TaxSubtype = TransactionTaxInfoItem.TaxInfoSubtypeEnum.VatSubtype Then
					retVal += transactionTaxItem.TaxAmount
				End If
			Next

			Return retVal
		End Function

		Public Function GetSalesTaxTotal() As Decimal
			Dim transactionTaxItem As TransactionTaxInfoItem
			Dim retVal As Decimal = 0

			For Each transactionTaxItem In _taxItems
				If transactionTaxItem.TaxSubtype = TransactionTaxInfoItem.TaxInfoSubtypeEnum.SalesTaxSubtype Then
					retVal += transactionTaxItem.TaxAmount
				End If
			Next

			Return retVal
		End Function

		Public Function SameValueAs(candidate As TransactionTaxInfo) As Boolean Implements [Shared].IValueObject(Of TransactionTaxInfo).SameValueAs
		End Function
	End Class
End Namespace