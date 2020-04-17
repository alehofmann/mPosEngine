Imports System.Runtime.Serialization

Namespace CommObjects
	<DataContract()>
	Public Class ResponseHeader
		Private _commandSuccess As Boolean
		Private _errorDescription As String

		<DataMember(Name:="errorDescription")>
		Public Property ErrorDescription() As String
			Get
				Return _errorDescription
			End Get
			Set(ByVal value As String)
				_errorDescription = value
			End Set
		End Property

		<DataMember(Name:="commandSuccess")>
		Public Property CommandSuccess() As Boolean
			Get
				Return _commandSuccess
			End Get
			Set(ByVal value As Boolean)
				_commandSuccess = value
			End Set
		End Property
	End Class
End Namespace