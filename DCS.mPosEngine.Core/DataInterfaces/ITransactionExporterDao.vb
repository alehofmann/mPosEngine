Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain

Namespace DataInterfaces
    Public Interface ITransactionExporterDao
        Inherits IGenericDao(Of DCS.mPosEngine.Core.Domain.ExternalSales.ExternalSystem, Integer)

    End Interface
End Namespace
