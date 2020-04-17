Imports DCS.mPosEngine.Core.Domain.Sales

Namespace DataInterfaces
    Public Interface IExternalSystemConnector
		Function CommitOperation(ByVal operation As ProductSellOperation, Optional ByRef errorMesssage As String = "", Optional ByVal paymentId As Integer = 0) As Boolean
	End Interface
End Namespace
