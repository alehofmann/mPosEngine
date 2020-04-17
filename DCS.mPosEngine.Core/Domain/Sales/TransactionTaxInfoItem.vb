Imports DCS.mPosEngine.Core.Domain.Shared

Namespace Domain.Sales
    Public Class TransactionTaxInfoItem
        Implements IValueObject(Of TransactionTaxInfoItem)
        Private _taxName As String
        Private _taxAmount As Decimal
        Private _taxTypeId As Integer

        Public Enum TaxInfoSubtypeEnum
            SalesTaxSubtype = 1
            VatSubtype = 2
        End Enum

        Private _taxSubtype As TaxInfoSubtypeEnum
        Public ReadOnly Property TaxSubtype() As TaxInfoSubtypeEnum
            Get
                Return _taxSubtype
            End Get
        End Property

        Public Property TaxTypeId() As Integer
            Get
                Return _taxTypeId
            End Get
            Set(ByVal value As Integer)
                _taxTypeId = value
            End Set
        End Property

        Public Sub New(ByVal subtype As TaxInfoSubtypeEnum, taxTypeId As Integer, ByVal taxName As String, ByVal taxAmount As Decimal)
            _taxTypeId = taxTypeId
            _taxName = taxName
            _taxAmount = taxAmount
            _taxSubtype = subtype
        End Sub


        Public Property TaxAmount() As Decimal
            Get
                Return _taxAmount
            End Get
            Set(ByVal value As Decimal)
                _taxAmount = value
            End Set
        End Property

        Public Property TaxName() As String
            Get
                Return _taxName
            End Get
            Set(ByVal value As String)
                _taxName = value
            End Set
        End Property

        Public Function SameValueAs(candidate As TransactionTaxInfoItem) As Boolean Implements [Shared].IValueObject(Of TransactionTaxInfoItem).SameValueAs
        End Function

    End Class
End Namespace
