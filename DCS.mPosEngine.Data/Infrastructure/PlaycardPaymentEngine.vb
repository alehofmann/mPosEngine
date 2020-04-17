Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.mPosEngine.Core.Domain.Sales.Payment

Namespace Infrastructure
    Public Class PlaycardPaymentEngine
        Implements IPaymentEngine

        Private ReadOnly _cardManager As ICardManager

        Public Sub New()
            _cardManager = CardManagerFactory.GetCardManager
        End Sub

        Public Function AuthorizePayment(payItem As Core.Domain.Sales.Payment.PayItem, ByVal onlyAuthorize As Boolean) As Core.DataInterfaces.IPaymentEngine.AuthorizeStatusEnum Implements Core.DataInterfaces.IPaymentEngine.AuthorizePayment
            Dim cardCounterBalance As Decimal
            If payItem Is Nothing Then
                Throw New ArgumentNullException("payItem")
            End If

            If payItem.Amount > 0 Then
                If Not payItem.Currency.PaymodeType = PlaycardBase.Core.PosDomain.Currency.PaymodeTypesEnum.PlaycardPaymodeType Then
                    Throw New ArgumentException("Currency  [" & payItem.Currency.Name & "] must be of type 'Playcard' in order to authorize")
                End If

                If Not _cardManager.IsCardSold(CType(payItem.PaymodeData, PlaycardData).CardNumber) Then
                    Return IPaymentEngine.AuthorizeStatusEnum.UnsoldPlaycard
                End If

                cardCounterBalance = _cardManager.GetCardCounterAmount(CType(payItem.PaymodeData, PlaycardData).CardNumber, payItem.Currency.CounterType.Id)
                If cardCounterBalance < payItem.Amount Then
                    Return IPaymentEngine.AuthorizeStatusEnum.NotEnoughPlaycardBalance
                Else
                    If Not onlyAuthorize Then
                        _cardManager.DebitFromCard(CType(payItem.PaymodeData, PlaycardData).CardNumber, payItem.Currency.CounterType.Id, payItem.Amount)
                    End If

                    Return IPaymentEngine.AuthorizeStatusEnum.PaymentAuthorized
                    End If
            Else
                    Return IPaymentEngine.AuthorizeStatusEnum.PaymentAuthorized
            End If
        End Function
    End Class
End Namespace
