Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto
    <JsonObject()> _
    <DataContract()> _
    Public Class LoginDataDto

        Private _cashierId As Integer
        Private _cashierLoginName As String
        Private _cashierFirstName As String
        Private _cashierLastName As String
        Private _cashierCardNumber As Integer
        Private _resultCode As ResultCodesEnum

        <DataMember(Name:="resultCode")> _
        Public Property ResultCode() As ResultCodesEnum
            Get
                Return _resultCode
            End Get
            Set(ByVal value As ResultCodesEnum)
                _resultCode = value
            End Set
        End Property

        Public Enum ResultCodesEnum
            LoginSuccess = 0
            LoginDeniedInvalidCredentials = 1
            LoginDeniedNoOpenTreasurySession = 2
            LoginInvalidTreasuryCashierId = 3
        End Enum

        <DataMember(Name:="id")> _
        Public Property CashierId() As Integer
            Get
                Return _cashierId
            End Get
            Set(ByVal value As Integer)
                _cashierId = value
            End Set
        End Property

        <DataMember(Name:="loginName")> _
        Public Property CashierLoginName() As String
            Get
                Return _cashierLoginName
            End Get
            Set(ByVal value As String)
                _cashierLoginName = value
            End Set
        End Property

        <DataMember(Name:="fName")> _
        Public Property CashierFirstName() As String
            Get
                Return _cashierFirstName
            End Get
            Set(ByVal value As String)
                _cashierFirstName = value
            End Set
        End Property

        <DataMember(Name:="lName")> _
        Public Property CashierLastName() As String
            Get
                Return _cashierLastName
            End Get
            Set(ByVal value As String)
                _cashierLastName = value
            End Set
        End Property

        <DataMember(Name:="cardNro")> _
        Public Property CashierCardNumber() As Integer
            Get
                Return _cashierCardNumber
            End Get
            Set(ByVal value As Integer)
                _cashierCardNumber = value
            End Set
        End Property
    End Class
End Namespace