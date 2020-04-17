Imports Newtonsoft.Json
Imports System.Runtime.Serialization

Namespace Dto

    <JsonObject()> _
    <DataContract()> _
    Public Class RegisterDeviceResponseDto

        Private _resultCode As ResultCodesEnum
        Private _deviceId As Long
        Private _deviceName As String

        Public Enum ResultCodesEnum
            RegisterOk = 0
            SerialAlreadyRegistered = 1
            InvalidPairCode = 2
        End Enum

        <DataMember(Name:="deviceName")> _
        Public Property DeviceName() As String
            Get
                Return _deviceName
            End Get
            Set(ByVal value As String)
                _deviceName = value
            End Set
        End Property

        <DataMember(Name:="deviceId")> _
        Public Property DeviceId() As Long
            Get
                Return _deviceId
            End Get
            Set(ByVal value As Long)
                _deviceId = value
            End Set
        End Property

        <DataMember(Name:="resultCode")> _
        Public Property ResultCode() As ResultCodesEnum
            Get
                Return _resultCode
            End Get
            Set(ByVal value As ResultCodesEnum)
                _resultCode = value
            End Set
        End Property



    End Class
End Namespace
