Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain

Namespace DataInterfaces
    Public Interface IConfigDao
        Inherits IGenericDao(Of PosSetting, Long)

        Function GetConfigForPos(ByVal posName As String) As IList(Of PosSetting)
    End Interface
End Namespace
