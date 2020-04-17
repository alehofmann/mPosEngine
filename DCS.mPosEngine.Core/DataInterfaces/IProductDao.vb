Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain
Imports DCS.mPosEngine.Core.Domain.Sales

Namespace DataInterfaces
    Public Interface IProductDao
        Inherits IGenericDao(Of Product, Long)

    End Interface
End Namespace
