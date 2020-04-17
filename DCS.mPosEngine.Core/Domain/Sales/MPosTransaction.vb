Imports DCS.ProjectBase.Core
Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment
Imports DCS.mPosEngine.Core.Domain.ExternalSales
Imports DCS.mPosEngine.Core.Domain.Sales.Payment
Imports DCS.PlaycardBase.Services

Namespace Domain.Sales
    Public Class InvalidPaymentAmountException
        Inherits ApplicationException
        Public Sub New(ByVal expectedPaymentAmount As Decimal, ByVal actualPaymentAmount As Decimal)
            MyBase.New("Payment total of " & actualPaymentAmount & " is different than MPosTransaction total of " & expectedPaymentAmount)
        End Sub
    End Class

    Public Class MPosTransaction
		Inherits DomainObject(Of Long)

		'Private _operations As ISet(Of MPosOperation) = New HashSet(Of MPosOperation)
		Private _taxExempt As Boolean = False
		Private _paymentData As PaymentData
		Private _transactionState As TransactionStateEnum = TransactionStateEnum.NotInitialized
		Private _openDate As Date
		'Private _operatorId As Integer
		'Private _posName As String
		Private _invoiceNumber As String
		Private _discountEngine As DiscountEngine
		Private _discountsApplied As Boolean = False

        Public Property InvoiceNumber() As String
            Get
                Return _invoiceNumber
            End Get
            Set(value As String)
                _invoiceNumber = value
            End Set
        End Property

        Public Sub New(operatorIdParam As Integer, posNameParam As String, invoiceNumber As String, Optional taxExempt As Boolean = False, Optional discountEngine As DCS.PlaycardBase.Services.DiscountEngine = Nothing)
            _taxExempt = taxExempt
            _invoiceNumber = invoiceNumber
            OperatorId = operatorIdParam
            PosName = posNameParam
            _discountEngine = discountEngine
        End Sub

        Private Sub New()
		End Sub
		Public Property PosName() As String
		Public Property OperatorId() As Integer
		Public Property IsTaxExempt() As Boolean
		Public Property Operations() As ISet(Of MPosOperation) = New HashSet(Of MPosOperation)

		Public Enum TransactionStateEnum
			NotInitialized = 0
			Open = 1
			Paid = 2
			Fulfilled = 3
		End Enum

		Public Sub ApplyDiscounts(discountList As IList(Of Integer))
			Dim discountAmount As Decimal

			If _transactionState <> TransactionStateEnum.Open Then
				Throw New InvalidOperationException("Cannot apply a discount to a transaction with a status of [" & [Enum].GetName(GetType(TransactionStateEnum), _transactionState) & "]")
			End If
			If _discountEngine Is Nothing Then
				Throw New InvalidOperationException("Cannot apply AddressOf discount, DiscountEngine has not been initialized")
			End If
			If _discountsApplied Then
				Throw New InvalidOperationException("Discounts have already been applied to this transaction")
			End If

			For Each operation As MPosOperation In Me.Operations
				For Each discountId In discountList
					discountAmount = _discountEngine.GetAmountToDiscount(discountId, operation.Product.Id, operation.Product.Price) * operation.Quantity

					If discountAmount > 0 Then
						operation.AddDiscount(discountId, discountAmount)
					End If
				Next
			Next

			_discountsApplied = True
		End Sub

		Public Function GetTotals() As TransactionTotals
			Return New TransactionTotals(Me)
		End Function

		Public Function GetLineitemWithProductId(productId As Long) As MPosOperation
			Return Operations.SingleOrDefault(Function(x) x.Product.Id = productId)

			'For Each li As MPosOperation In Operations
			'    If li.Product.Id = productId Then
			'        Return li
			'    End If
			'Next
			'Return Nothing
		End Function

        Public Function AddLineitem(settledPrice As Decimal, quantity As Decimal, product As DCS.PlaycardBase.Core.PosDomain.Product, Optional ByVal cardInfo As CardInfo = Nothing, Optional ByVal externalSystemData As ExternalSystem = Nothing) As MPosOperation
            Dim newLineitem As MPosOperation

            'Check preconditions***********************
            If _transactionState <> TransactionStateEnum.Open And _transactionState <> TransactionStateEnum.NotInitialized Then
                Throw New NotSupportedException("Cannot add a MPosOperation to a MPosTransaction with status " & _transactionState)
            End If

            'De estos chequeos habría que reveer cuales habria que mover a sus respectivas clases (como mPosOperation por ejemplo).
            'No se si tiene sentido chequear aca estos invariantes.

            If quantity = 0 Then Throw New ArgumentNullException("quantity")
            If product Is Nothing Then Throw New ArgumentNullException("product")

            If (product.NeedsCard Or product.SellsNewCard) And cardInfo Is Nothing Then
                Throw New NullCardAndCardNeededException
            End If

            If product.SellsNewCard And Math.Abs(quantity) <> 1 Then
                Throw New CannotAddMoreThanOneNewCardProductToLineitemException
            End If

            '******************************************

            If quantity > 0 Then
                If cardInfo IsNot Nothing AndAlso (product.NeedsCard And Not product.SellsNewCard And Not cardInfo.IsSold And Not HasSellOperationForCard(cardInfo.CardNumber)) Then
                    Throw New CardNotSoldException(cardInfo.CardNumber)
                End If

                newLineitem = New ProductSellOperation(settledPrice, quantity, product, IsTaxExempt, cardInfo, externalSystemData)
            Else
                'vaya uno a saber por que IsReturnable es un string
                If Not CBool(product.IsReturnable) Then
                    Throw New ProductNotReturnableException(product.Name)
                End If

                If (product.SellsNewCard Or product.NeedsCard) AndAlso Not cardInfo.IsSold Then
                    Throw New CardNotSoldException(cardInfo.CardNumber)
                End If

                newLineitem = New ProductReturnOperation(settledPrice, quantity, product, IsTaxExempt, cardInfo)
            End If

            If Operations.Contains(newLineitem) Then
                Operations.Remove(newLineitem)
            End If
            newLineitem.Transaction = Me
            Operations.Add(newLineitem)

            If _transactionState = TransactionStateEnum.NotInitialized Then
                _transactionState = TransactionStateEnum.Open
                _openDate = Now
            End If

            Return newLineitem
        End Function

        Private Function HasSellOperationForCard(ByVal cardnumber As Long)
			For Each operation As MPosOperation In Operations
				If operation.Product.SellsNewCard Then
					Return True
				End If
			Next
			Return False
		End Function

		Public Sub SetFulfilled()
			If _transactionState <> TransactionStateEnum.Paid Then
				Throw New NotSupportedException("Cannot fulfill a MPosTransaction with status " & _transactionState)
			End If

			_transactionState = TransactionStateEnum.Fulfilled
		End Sub

        Public ReadOnly Property PaymentData As PaymentData
            Get
                Return _paymentData
            End Get
        End Property

        Public Sub Pay(ByVal paymentDataParam As PaymentData)
            If _transactionState <> TransactionStateEnum.Open Then
                Throw New NotSupportedException("Cannot pay a MPosTransaction with status " & _transactionState)
            End If

            If GetTotals.TotalToPay <> paymentDataParam.GetTotalAmount Then
                Throw New InvalidPaymentAmountException(GetTotals.TotalToPay, paymentDataParam.GetTotalAmount)
            End If

            _paymentData = paymentDataParam

            _transactionState = TransactionStateEnum.Paid
        End Sub

        Public ReadOnly Property HasBeenFulfilled As Boolean
			Get
				Return _transactionState = TransactionStateEnum.Fulfilled
			End Get
		End Property

		Public Function IsFullyPaid() As Boolean
			Return _transactionState = TransactionStateEnum.Paid
		End Function

        Public Property OpenDate As Date
            Get
                Return _openDate
            End Get
            Private Set(value As Date)
                _openDate = value
            End Set
        End Property

        'Private Function GetTaxInfo() As TaxInfo
        '    Dim retVal As New TaxInfo
        '    Dim lineitem As MPosOperation



        '    For Each lineitem In Operations
        '        If lineitem.Product.ProductData.PaysTax Then
        '            'retVal.AddTaxItem(New TransactionTaxInfoItem(1, "Tax de mentira 10%", MPosOperation.Product.ProductData.Price * 0.1))
        '            retVal.AddTax(1, "Tax de mentira 10%", lineitem.Product.ProductData.Price * 0.1)
        '            'retVal.AddTaxItem(New TransactionTaxInfoItem(2, "Tax de mentira 20%", MPosOperation.Product.ProductData.Price * 0.2))
        '            retVal.AddTax(2, "Tax de mentira 20%", lineitem.Product.ProductData.Price * 0.2)
        '        End If

        '    Next

        '    Return retVal
        'End Function

        'Public Function GetTotals(ByVal mPosTransaction As MPosTransaction) As TransactionTotals
        '    Dim lineitem As MPosOperation
        '    Dim retVal As New TransactionTotals

        '    For Each lineitem In mPosTransaction.Operations
        '        retVal.Subtotal1 += lineitem.PriceSettled * lineitem.Quantity
        '    Next

        '    If Not mPosTransaction.TaxExempt Then
        '        retVal.TaxInfo = GetTaxInfo()
        '    End If

        '    Return retVal
        'End Function

        Public Overrides Function GetHashCode() As Integer

		End Function

	End Class


End Namespace