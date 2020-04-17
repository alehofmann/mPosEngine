Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace Dto
    <JsonObject()> _
    <DataContract()>
    Public Class CardConsolidateCommandDto
        Private _operatorId As Integer
        Private _mPosName As String
        Private _sourceCards As IList(Of Long)
        Private _destCard As Long

        <DataMember(Name:="destCard")> _
        Public Property DestCard() As Long
            Get
                Return _destCard
            End Get
            Set(ByVal value As Long)
                _destCard = value
            End Set
        End Property

        <DataMember(Name:="sourceCards")> _
        Public Property SourceCards() As IList(Of Long)
            Get
                Return _sourceCards
            End Get
            Set(ByVal value As IList(Of Long))
                _sourceCards = value
            End Set
        End Property

        <JsonProperty()> _
        <DataMember(Name:="mPosName")> _
        Public Property MPosName() As String
            Get
                Return _mPosName
            End Get
            Set(ByVal value As String)
                _mPosName = value
            End Set
        End Property

		<JsonProperty()>
		<DataMember(Name:="operatorId")>
		Public Property OperatorId() As Integer
			Get
				Return _operatorId
			End Get
			Set(ByVal value As Integer)
				_operatorId = value
			End Set
		End Property
	End Class
End Namespace