Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain.Sales

Namespace DataInterfaces
    Public Interface IMPosTransactionDao
        Inherits IGenericDao(Of MPosTransaction, Long)
    End Interface
End Namespace
