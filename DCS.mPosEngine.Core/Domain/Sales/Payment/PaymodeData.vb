Imports DCS.ProjectBase.Core
Imports DCS.PlaycardBase.Core.PosDomain

Namespace Domain.Sales.Payment
    Public MustInherit Class PaymodeData
        Inherits DomainObject(Of Long)

        Implements IPaymodeData

        Public Sub New()

        End Sub

        Public Shared Function GetPaymodeData(ByVal paymodeType As Currency.PaymodeTypesEnum, Optional ByVal creditCardNumber As String = vbNullString, Optional ByVal creditCardType As String = vbNullString, Optional ByVal creditCardDebugInfo As String = vbNullString, Optional ByVal creditCardAuthorizerReference As String = vbNullString, Optional ByVal playcardNumber As Long = 0) As IPaymodeData
            Select Case paymodeType
                Case Currency.PaymodeTypesEnum.CashPaymodeType
                    Return New CashPaymodeData
                Case Currency.PaymodeTypesEnum.CreditCardPaymodeType
                    Return New CreditCardData(creditCardNumber, creditCardType, creditCardAuthorizerReference, creditCardDebugInfo)
                Case Currency.PaymodeTypesEnum.PlaycardPaymodeType
                    Return New PlaycardData(playcardNumber)
                Case Else
                    Return New UnknownPaymodeData

            End Select
        End Function

        Public Overrides Function GetHashCode() As Integer

        End Function
    End Class
End Namespace
