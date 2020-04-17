Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain

Namespace DataInterfaces
    Public Interface IDeviceDao
        Inherits IGenericDao(Of PosDevice, Long)

        Function GetDeviceBySerial(ByVal serial As String) As PosDevice
        Function GetDeviceByName(ByVal name As String) As PosDevice
        Function GetDeviceByPairCode(ByVal pairCode As String) As PosDevice
    End Interface
End Namespace
