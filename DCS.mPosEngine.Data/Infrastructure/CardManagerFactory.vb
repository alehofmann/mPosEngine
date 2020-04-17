Imports DCS.mPosEngine.Core.DataInterfaces

Namespace Infrastructure
    Public Class CardManagerFactory
		Shared Function GetCardManager() As ICardManager
			Return New PlaycardBaseCardManager
		End Function
	End Class
End Namespace