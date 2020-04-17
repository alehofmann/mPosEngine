Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto

    <JsonObject()> _
    <DataContract()> _
    Public Class CardTransferResponseDto

        Private _resultCode As ResultCodesEnum
        Private _invalidCardNumber As Long

        Public Enum ResultCodesEnum
            TransferSuccess = 0
            InvalidSourceCard = 1
            InvalidDestinationCard = 2
        End Enum

        Public Sub New()

        End Sub
        Public Sub New(ByVal resultCode As ResultCodesEnum, Optional ByVal invalidCardNumber As Long = 0)
            _resultCode = resultCode
            _invalidCardNumber = invalidCardNumber
        End Sub

        <JsonProperty()>
        <DataMember(Name:="invalidCardNumber")> _
        Public Property InvalidCardNumber As Long
            Get
                Return _invalidCardNumber
            End Get
            Set(value As Long)
                _invalidCardNumber = value
            End Set
        End Property

        <JsonProperty()>
        <DataMember(Name:="resultCode")> _
        Public Property ResultCode As ResultCodesEnum
            Get
                Return _resultCode
            End Get
            Set(value As ResultCodesEnum)
                _resultCode = value
            End Set
        End Property
    End Class
End Namespace
