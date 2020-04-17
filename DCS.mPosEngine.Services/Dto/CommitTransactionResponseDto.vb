Imports System.Runtime.Serialization

Namespace Dto
	<DataContract()>
	Public Class CommitTransactionResponseDto
		Private _resultCode As ResultCodesEnum
		Private _errorDescription As String
		Private _errorCode As Integer
		Private _param1 As String
		Private _param2 As String
		Private _param3 As String

		Public Enum ResultCodesEnum
			CommitSuccess = 0
			NoActiveTreasurySession = 1
			PaymentFailedUnsoldPlaycard = 2
			PaymentFailedNotEnoughPlaycardBalance = 3
			CardNotSold = 4
			NotEnoughBalance = 5
			ProductNotReturnable = 6
            ReportToExternalSystemFailed = 7
            ErrorAtProcessVoucher = 8
            ErrorAtRedeemVoucher = 9
        End Enum

		Public Sub New(ByVal resultCode As ResultCodesEnum, Optional ByVal param1 As String = vbNullString, Optional param2 As String = vbNullString, Optional param3 As String = vbNullString)
			'_errorDescription = ErrorDescription
			_errorCode = ErrorCode
			_resultCode = resultCode
			_param1 = param1
			_param2 = param2
			_param3 = param3
		End Sub

		Public Property Param3() As String
			Get
				Return _param3
			End Get
			Set(ByVal value As String)
				_param3 = value
			End Set
		End Property

		Public Property Param2() As String
			Get
				Return _param2
			End Get
			Set(ByVal value As String)
				_param2 = value
			End Set
		End Property

		Public Property Param1() As String
			Get
				Return _param1
			End Get
			Set(ByVal value As String)
				_param1 = value
			End Set
		End Property

		<DataMember(Name:="errorCode")>
		Public Property ErrorCode() As Integer
			Get
				Return _errorCode
			End Get
			Set(ByVal value As Integer)
				_errorCode = value
			End Set
		End Property

		'<DataMember(Name:="errorDescription")> _
		'Public Property ErrorDescription() As String
		'    Get
		'        Return _errorDescription
		'    End Get
		'    Set(ByVal value As String)
		'        _errorDescription = value
		'    End Set
		'End Property

		<DataMember(Name:="resultCode")>
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