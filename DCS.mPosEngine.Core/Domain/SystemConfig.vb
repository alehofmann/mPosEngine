Imports DCS.ProjectBase.Core

Namespace Domain
	Public Class SystemConfig
		Inherits DomainObject(Of Integer)

		Public Property Program As String
		Public Property Section As String
		Public Property Key As String
		Public Property Value As String

		Public Overrides Function GetHashCode() As Integer
		End Function
	End Class
End Namespace