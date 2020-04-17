Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain
Imports DCS.mPosEngine.Core.Domain.Pagesets

Namespace DataInterfaces
    Public Interface IProductPageDao
        Inherits IGenericDao(Of ProductPage, Integer)

        Function GetPagesForPos(ByVal posName As String) As IList(Of ProductPage)

    End Interface
End Namespace
