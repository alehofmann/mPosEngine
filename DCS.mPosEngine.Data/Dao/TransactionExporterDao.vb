Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.ProjectBase.Data

Namespace Dao
    Public Class TransactionExporterDao
        Inherits GenericDao(Of DCS.mPosEngine.Core.Domain.ExternalSales.ExternalSystem, Integer)
        Implements ITransactionExporterDao
    End Class
End Namespace
