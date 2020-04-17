Imports System.Runtime.Serialization

Namespace Dto

    <DataContract()>
Public Class PassportItemDto
        Private _passportName As String
        Private _passportQuantity As Decimal

        Public Sub New(ByVal name As String, ByVal quantity As Decimal)
            _passportName = name
            _passportQuantity = quantity
        End Sub
        <DataMember(Name:="name")> _
        Public Property PassportName() As String
            Get
                Return _passportName
            End Get
            Set(ByVal value As String)
                _passportName = value
            End Set
        End Property

        <DataMember(Name:="qty")> _
        Public Property PassportQuantity() As Decimal
            Get
                Return _passportQuantity
            End Get
            Set(ByVal value As Decimal)
                _passportQuantity = value
            End Set
        End Property
    End Class
End Namespace
