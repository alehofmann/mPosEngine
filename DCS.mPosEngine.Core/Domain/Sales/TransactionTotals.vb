
Imports DCS.mPosEngine.Core.Domain.Shared
Imports NHibernate.Util

Namespace Domain.Sales
	Public Class TransactionTotals
		Implements IValueObject(Of TransactionTotals)

		'Private _subTotal1 As Decimal
		'Private _totalDiscount As Decimal = New Decimal(0)
		'Private _taxInfo As TaxInfo = New TaxInfo

		Private _transaction As MPosTransaction

        Public Sub New(ByVal transaction As MPosTransaction)
            _transaction = transaction

            '_subTotal1 = 0
            'For Each operation As MPosOperation In transaction.Operations
            '    _subTotal1 += operation.Subtotal
            '    _taxInfo.AddTaxInfo(operation.TaxInfo)
            'Next
        End Sub

        Public ReadOnly Property TransactionTaxInfo() As TransactionTaxInfo
			Get
				'Return _taxInfo
				Dim retVal As New TransactionTaxInfo

				For Each operation As MPosOperation In _transaction.Operations
					retVal.AddOperationTaxInfo(operation.TaxInfo)
				Next

				Return retVal
			End Get
		End Property

		Public ReadOnly Property TotalToPay() As Decimal
			Get
				'Return Decimal.Round(Subtotal2 + TotalSalesTax, Configuration.GetInteger("decimalPlaces", 2))
				Dim retVal As Decimal

				For Each operation As MPosOperation In _transaction.Operations
					retVal += operation.TotalToPay
				Next

                '   Braian 190906 Esto no descontaba el verdadero descuento (Cómo funcionó hasta ahora?)
                'retVal -= GetTransactionwideDiscountAmount()
                'retVal -= TotalDiscount

                'Return Decimal.Round(retVal, Configuration.GetInteger("decimalPlaces", 2))
                Return retVal
			End Get
		End Property

        '   Braian 190906 Comento, RAROOU
        'Public Function GetTransactionwideDiscountAmount() As Decimal
        '	Return 0
        'End Function

        Public ReadOnly Property TotalTax As Decimal
			Get
				Return TransactionTaxInfo.GetSalesTaxTotal + TransactionTaxInfo.GetVatTotal
			End Get
		End Property

		Public ReadOnly Property TotalSalesTax() As Decimal
			Get
				'Return _taxInfo.GetSalesTaxTotal
				Return TransactionTaxInfo.GetSalesTaxTotal
				'Return TransactionTaxInfo.GetSalesTaxTotal + TransactionTaxInfo.GetVatTotal
			End Get

		End Property

		Public ReadOnly Property Subtotal2() As Decimal
			Get
				'Return Subtotal1 - TotalDiscount
				Dim retVal As Decimal
				For Each operation As MPosOperation In _transaction.Operations
					retVal += operation.Subtotal2
				Next

				Return retVal
			End Get
		End Property

		Public ReadOnly Property TotalDiscount() As Decimal
			Get
				'            Dim totalOperationwideDiscountAmount As Decimal = 0
				'For Each operation As MPosOperation In _transaction.Operations
				'	totalOperationwideDiscountAmount += operation.OperationDiscountAmount
				'Next

				'Return totalOperationwideDiscountAmount + GetTransactionwideDiscountAmount()

				Dim totalDiscountAmount As Decimal = 0
				_transaction.Operations.ForEach(Sub(op) totalDiscountAmount += op.TotalDiscountAmount)
				Return totalDiscountAmount
			End Get
		End Property

		Public ReadOnly Property Subtotal1() As Decimal
			Get
				'Return _subTotal1
				'Es el subtotal sin IVA/TAX

				Dim retVal As Decimal = 0
				For Each operation As MPosOperation In _transaction.Operations
					retVal += operation.Subtotal
				Next

				'Y asi calcula el neto
				'Ahora hace esta resta/discriminación directamente adentro de operation.subtotal
				'Return retVal - TransactionTaxInfo.GetVatTotal
				Return retVal
			End Get
		End Property

		Public Function SameValueAs(candidate As TransactionTotals) As Boolean Implements [Shared].IValueObject(Of TransactionTotals).SameValueAs
		End Function
	End Class
End Namespace