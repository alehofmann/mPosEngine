Imports DCS.ProjectBase.Core
Imports DCS.mPosEngine.Core.Domain.Shared

Namespace Domain.Sales
    Public Class OperationTaxInfoItem
        Inherits DomainObject(Of Long)

        Private _taxName As String
        Private _taxAmount As Decimal
        Private _taxTypeId As Integer
        Private _parentOperation As MPosOperation

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

        Private Property ParentOperation As MPosOperation
            Get
                Return _parentOperation
            End Get
            Set(value As MPosOperation)
                _parentOperation = value
            End Set
        End Property

        Public Sub New(ByVal parentOperation As MPosOperation, ByVal subtype As TaxInfoSubtypeEnum, taxTypeId As Integer, ByVal taxName As String, ByVal taxAmount As Decimal)
            _taxTypeId = taxTypeId
            _taxName = taxName
            _taxAmount = taxAmount
            _taxSubtype = subtype
            _parentOperation = parentOperation
        End Sub

        'For ORM
        Private Sub New()

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

    

        Public Overrides Function GetHashCode() As Integer
            Return (GetType(OperationTaxInfoItem).FullName & TaxTypeId.GetHashCode).GetHashCode
        End Function
    End Class
End Namespace
