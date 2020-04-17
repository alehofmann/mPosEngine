Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain.Sales

Namespace Dao

    Public Class MPosTransactionDao
        Inherits GenericDao(Of mpostransaction, Long)

        Implements IMPosTransactionDao
    End Class
End NameSpace