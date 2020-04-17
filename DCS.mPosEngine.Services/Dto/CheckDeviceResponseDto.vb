Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto
    <JsonObject()> _
    <DataContract()>
    Public Class CheckDeviceResponseDto

        Private _deviceId As Long
        Private _deviceName As String
        Private _resultCode As ResultCodesEnum
        Private _computerId As Integer

        Public Enum ResultCodesEnum
            DeviceValidAndEnabled = 0
            InvalidSerialNumber = 1
            DeviceIsDisabled = 2
        End Enum

        <DataMember(Name:="computerId")> _
        <JsonProperty("computerId")> _
        Public Property ComputerId As Integer
            Get
                Return _computerId
            End Get
            Set(value As Integer)
                _computerId = value
            End Set
        End Property

        <DataMember(Name:="resultCode")> _
        <JsonProperty("resultCode")> _
        Public Property ResultCode() As ResultCodesEnum
            Get
                Return _resultCode
            End Get
            Set(ByVal value As ResultCodesEnum)
                _resultCode = value
            End Set
        End Property
        <DataMember(Name:="deviceName")> _
        <JsonProperty("deviceName")> _
        Public Property DeviceName() As String
            Get
                Return _deviceName
            End Get
            Set(ByVal value As String)
                _deviceName = value
            End Set
        End Property

        <DataMember(Name:="deviceId")> _
        <JsonProperty("deviceId")> _
        Public Property DeviceId() As Long
            Get
                Return _deviceId
            End Get
            Set(ByVal value As Long)
                _deviceId = value
            End Set
        End Property

    End Class
End Namespace
