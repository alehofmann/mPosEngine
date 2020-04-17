'Imports DCS.mPosEngine.Core.Domain.Sales
'Imports DCS.PlaycardBase.Core.PosDomain

'Namespace Domain.Sales
'	Public Class OperationDiscountInfo
'		Private _discountsApplied As List(Of OperationDiscountItem)
'		Private _parent As MPosOperation

'		Public Sub New(parent As MPosOperation, discountsApplied As List(Of Discount))
'			_parent = Parent

'			_discountsApplied = New List(Of OperationDiscountItem)()
'			DiscountsApplied.ForEach(Sub(x) _discountsApplied.Add(New OperationDiscountItem(_parent, x.Id, x.DiscountAmount)))
'		End Sub

'		Public Sub New()
'			_discountsApplied = New List(Of OperationDiscountItem)()
'		End Sub

'		Public Sub AddItem(Discount As OperationDiscountItem)
'			If _discountsApplied IsNot Nothing Then
'				_discountsApplied.Add(Discount)
'			End If
'		End Sub

'		Public Function GetTotalDiscount() As Decimal
'			If _discountsApplied Is Nothing Then Return 0

'			Dim total As Decimal
'			_discountsApplied.ForEach(Sub(x) total += x.Amount)
'			Return total
'		End Function
'	End Class
'End Namespace