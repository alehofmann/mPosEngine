Imports DCS.ProjectBase.Core

Namespace Domain.Sales
	Public Class OperationDiscountItem
		Inherits DomainObject(Of Long)

		Private _parent As MPosOperation
		Private _discountId As Integer
		Private _discountAmount As Decimal

		Private Sub New()

		End Sub

		Public Sub New(parent As MPosOperation, discountId As Integer, discountAmountApplied As Decimal)
			_parent = parent
			_discountId = discountId
			_discountAmount = discountAmountApplied
		End Sub

		Public Sub New(discountId As Integer, discountAmountApplied As Decimal)
			_discountId = discountId
			_discountAmount = discountAmountApplied
		End Sub

		Public Overrides Function GetHashCode() As Integer
			Return (GetType(OperationDiscountItem).FullName & DiscountId.GetHashCode).GetHashCode
		End Function

		Public Property DiscountId() As Integer
			Get
				Return _discountId
			End Get
			Set(value As Integer)
				_discountId = value
			End Set
		End Property

		Public Property Amount() As Decimal
			Get
				Return _discountAmount
			End Get
			Set(value As Decimal)
				_discountAmount = value
			End Set
		End Property

		Private Property ParentOperation As MPosOperation
			Get
				Return _parent
			End Get
			Set(value As MPosOperation)
				_parent = value
			End Set
		End Property
	End Class
End Namespace