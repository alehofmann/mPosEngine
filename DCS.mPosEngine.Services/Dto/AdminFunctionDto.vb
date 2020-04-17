Imports Newtonsoft.Json
Imports System.Runtime.Serialization

Namespace Dto
	<JsonObject()>
	<DataContract()>
	Public Class AdminFunctionDto

		Private _functionName As String
		Private _authenticationRequired As Boolean
		Private _functionId As Integer
		Private _securityActionId As Integer
		Private _thumbUrl As String

		<DataMember(Name:="securityActionId")>
		<JsonProperty()>
		Public Property SecurityActionId() As Integer
			Get
				Return _securityActionId
			End Get
			Set(ByVal value As Integer)
				_securityActionId = value
			End Set
		End Property


		<DataMember(Name:="functionId")>
		<JsonProperty()>
		Public Property FunctionId() As Integer
			Get
				Return _functionId
			End Get
			Set(ByVal value As Integer)
				_functionId = value
			End Set
		End Property

		<DataMember(Name:="authenticationRequired")>
		<JsonProperty()>
		Public Property AuthenticationRequired() As Boolean
			Get
				Return _authenticationRequired
			End Get
			Set(ByVal value As Boolean)
				_authenticationRequired = value
			End Set
		End Property

		<DataMember(Name:="functionName")>
		<JsonProperty()>
		Public Property FunctionName() As String
			Get
				Return _functionName
			End Get
			Set(ByVal value As String)
				_functionName = value
			End Set
		End Property

		<DataMember(Name:="thumbUrl")>
		<JsonProperty()>
		Public Property ThumbUrl() As String
			Get
				Return _thumbUrl
			End Get
			Set(ByVal value As String)
				_thumbUrl = value
			End Set
		End Property
	End Class
End Namespace