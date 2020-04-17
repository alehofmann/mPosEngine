Imports System.Runtime.Serialization

Namespace CommObjects
	<DataContract()>
	Public Class SuccessResponse

		<DataMember(Name:="success")>
		Public Property Success As Boolean

		Public Sub New(result As Boolean)
			Success = result
		End Sub
	End Class
End Namespace