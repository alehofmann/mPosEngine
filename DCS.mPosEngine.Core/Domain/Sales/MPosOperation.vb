Imports DCS.PlaycardBase.Core.CardDomain
Imports DCS.ProjectBase.Core
Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment
Imports DCS.mPosEngine.Core.Domain.ExternalSales
Imports DCS.PlaycardBase.Core.PosDomain
Imports NHibernate.Util

Namespace Domain.Sales
	Public MustInherit Class MPosOperation
		Inherits DomainObject(Of Long)

		Private _product As DCS.PlaycardBase.Core.PosDomain.Product
		Private _quantity As Decimal
		Private _priceSettled As Decimal
		Private _card As CardInfo
		Private _taxInfo As OperationTaxInfo
		Private _fulfillmentData As OperationFulfillmentData
		Private _fulfilled As Boolean = False
		Private _transaction As MPosTransaction
		'Private _totalDiscountAmount As Decimal

		Property DiscountItems As ISet(Of OperationDiscountItem) = New HashSet(Of OperationDiscountItem)

		'Private _taxApplied As Boolean = False

		Public MustOverride ReadOnly Property OperationType As Byte
		Public MustOverride ReadOnly Property SellsNewCard As Boolean

		Private _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

		Public Overrides Function ToString() As String
			Return Quantity & "x" & _product.Name
		End Function

		Protected Sub New(priceSettled As Decimal, quantity As Decimal, product As DCS.PlaycardBase.Core.PosDomain.Product, taxExemptTransaction As Boolean, Optional ByVal cardInfo As CardInfo = Nothing)
			If priceSettled < 0 Then
				Throw New ArgumentException("priceSettled must be 0 or greater", "priceSettled")
			End If

			If product Is Nothing Then
				Throw New ArgumentNullException("product")
			End If

			If (product.SellsNewCard Or product.NeedsCard) And cardInfo Is Nothing Then
				Throw New ArgumentNullException("cardInfo", "Card parameter is needed in order to build an operation for product id " & product.Id)
			End If

			_priceSettled = priceSettled
			_quantity = quantity
			_product = product
			_card = cardInfo

			_log.Debug("Creating 'TaxInfo'")
			'_taxInfo = New TaxInfo(Me.Subtotal2, product.TaxTypes)
			_taxInfo = New OperationTaxInfo(Me, product.TaxTypes)

			'Hasta aca es comun a todos los operations. De aca en adelante es propio de un ProductSellOperation
			'|
		End Sub

		Public Sub SetFulfilled()
			If _fulfilled Then
				Throw New InvalidOperationException("Operation already fulfilled")
			End If
		End Sub

		Private Property TaxItems As ISet(Of OperationTaxInfoItem)
			Get
				Return TaxInfo.TaxItems
			End Get
			Set(value As ISet(Of OperationTaxInfoItem))
				_taxInfo = New OperationTaxInfo
				_taxInfo.TaxItems = value
			End Set
		End Property

		Protected Property FulfillmentData() As OperationFulfillmentData
			Get
				Return _fulfillmentData
			End Get
			Set(ByVal value As OperationFulfillmentData)
				_fulfillmentData = value
			End Set
		End Property

		Public Function GetFulfillmentData() As OperationFulfillmentData
			Return _fulfillmentData
		End Function

		Friend Property Transaction As MPosTransaction
			Set(value As MPosTransaction)
				_transaction = value
			End Set
			Private Get
				Return _transaction
			End Get
		End Property

		Public ReadOnly Property IsCardRelated As Boolean
			Get
				'Return _card IsNot Nothing
				Return _product.NeedsCard Or _product.SellsNewCard
			End Get
		End Property

		Public Property Card As CardInfo
			Get
				Return _card
			End Get
			Private Set(value As CardInfo)
				_card = value
			End Set
		End Property

		Public Property PriceSettled() As Decimal

			Get
				Return _priceSettled
			End Get
			Private Set(value As Decimal)
				_priceSettled = value
			End Set
		End Property

		Public Property Quantity() As Decimal
			Get
				Return _quantity
			End Get
			Private Set(value As Decimal)
				_quantity = value
			End Set
		End Property

		Public Property Product() As DCS.PlaycardBase.Core.PosDomain.Product
			Get
				Return _product
			End Get
			Private Set(value As DCS.PlaycardBase.Core.PosDomain.Product)
				_product = value
			End Set
		End Property

		Protected Sub New()
		End Sub

		Public ReadOnly Property TaxInfo() As OperationTaxInfo
			Get
				Return _taxInfo
			End Get
		End Property

		Public ReadOnly Property TotalToPay() As Decimal
			Get
				'Return Subtotal + TaxInfo.GetVatTotal + TaxInfo.GetSalesTaxTotal - OperationDiscountAmount
				Return Subtotal + TaxInfo.GetVatTotal + TaxInfo.GetSalesTaxTotal - TotalDiscountAmount
			End Get
		End Property

		Public ReadOnly Property TotalSalesTax() As Decimal
			Get
				'Return Decimal.Round(TaxInfo.GetSalesTaxTotal, Configuration.GetInteger("decimalPlaces", 2))
				Return TaxInfo.GetSalesTaxTotal + TaxInfo.GetVatTotal()
			End Get
		End Property

		'Public Sub ApplyDiscount(ByVal discountId As Integer, ByVal discountAmount As Decimal)
		'    _discountAmountAmount = discountAmount
		'    _appliedDiscountId = discountId
		'End Sub

		Public Overrides Function GetHashCode() As Integer
			If Not _card Is Nothing Then
				Return (GetType(MPosOperation).FullName & _product.Id.GetHashCode & _card.CardNumber.GetHashCode).GetHashCode
			Else
				Return (GetType(MPosOperation).FullName & _product.Id.GetHashCode).GetHashCode
			End If
		End Function

		Public ReadOnly Property Fulfilled As Boolean
			Get
				Return _fulfilled
			End Get
		End Property

		Public ReadOnly Property Subtotal2 As Decimal
			Get
				'Return Subtotal - OperationDiscountAmount
				Return Subtotal - TotalDiscountAmount
			End Get
		End Property

		Public Function GetTaxBase() As Decimal
			'Return (Quantity * PriceSettled) - OperationDiscountAmount
			Return (Quantity * PriceSettled) - TotalDiscountAmount
		End Function

		Public ReadOnly Property Subtotal As Decimal
			Get
				'Return Decimal.Round((Quantity * PriceSettled) - TaxInfo.GetVatTotal, Configuration.GetInteger("decimalPlaces", 2))
				'Return Decimal.Round((Quantity * PriceSettled) - TaxInfo.GetVatTotal - TaxInfo.GetSalesTaxTotal - TotalDiscountAmount, Configuration.GetInteger("decimalPlaces", 2))
				Return Decimal.Round((Quantity * PriceSettled) - TaxInfo.GetVatTotal - TaxInfo.GetSalesTaxTotal, Configuration.GetInteger("decimalPlaces", 2))
			End Get
		End Property

		Public ReadOnly Property SubTotalWithoutDiscount As Decimal
			Get
				Return Subtotal - TotalDiscountAmount
			End Get
		End Property

		Public ReadOnly Property TotalDiscountAmount
			Get
				Return DiscountItems.Sum(Function(x) x.Amount)
			End Get
		End Property

		Public Sub AddDiscount(discountId As Integer, amount As Decimal)
			DiscountItems.Add(New OperationDiscountItem(Me, discountId, amount))
			Me.TaxInfo.RecalculateTax()
		End Sub
	End Class
End Namespace