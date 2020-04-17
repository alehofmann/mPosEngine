Imports DCS.PlaycardBase.Core.CardDomain
Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.mPosEngine.Core.Domain.Sales
Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment

Namespace DomainServices

    Public Class FulfillmentService
        Private _cardManager As ICardManager    
        Private _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Sub New(ByVal cardManager As ICardManager)
            _cardManager = cardManager
        End Sub
        Public Sub FulfillTransaction(ByVal mPosTransaction As MPosTransaction)
            Dim operationFulfillmentData As OperationFulfillmentData

            If mPosTransaction Is Nothing Then Throw New ArgumentNullException("mPosTransaction")

            _log.Info("Fulfilling " & mPosTransaction.Operations.Count & " operations")

            For Each operation As MPosOperation In mPosTransaction.Operations
                If operation.GetFulfillmentData.HasCardOperation Then
                    _log.Info("Commiting card charge")
                    CommitCardCharge(operation.GetFulfillmentData.CardOperation)
                    _log.Debug("Marking operation as fulfilled")
                    operation.SetFulfilled()
                End If
            Next
            _log.Debug("All operations fulfilled, marking transaction as fulfilled")
            mPosTransaction.SetFulfilled()
        End Sub

        Private Sub CommitCardCharge(ByVal operation As CardOperation)
            Dim amountInCard As Decimal

            If operation.SellsNewCard Then
                _log.Debug("Operation sells a new card, selling card then")
                _cardManager.SellCard(operation.Card.CardNumber)
            End If

            If operation.MovesCounters Then
                _log.Debug("Operation moves card counters, making so...")
                For Each counterMovement As CounterTransactionItem In operation.CounterTransaction.Items
                    If counterMovement.Amount < 0 Then                        
                        'It's a return
                        _log.Debug("Counter movement is a debit, getting card balance to find out if there is enough")
                        amountInCard = _cardManager.GetCardCounterAmount(operation.Card.CardNumber, counterMovement.CounterType.Id)
                        If amountInCard < counterMovement.Amount * -1 Then
                            'Not enough credits
                            _log.Debug("Counter balance is not enough, aborting card charge")
                            Throw New NotEnoughCreditsToReturnException(counterMovement.CounterType.Description, amountInCard, counterMovement.Amount * -1)
                        End If

                        _log.Debug("Debiting " & counterMovement.Amount & " " & counterMovement.CounterType.Description & " from card " & operation.Card.CardNumber)
                    Else
                        _log.Debug("Charging " & counterMovement.Amount & " " & counterMovement.CounterType.Description & " to card " & operation.Card.CardNumber)
                    End If

                    _cardManager.ChargeCard(operation.Card.CardNumber, counterMovement.CounterType.Id, counterMovement.Amount)
                Next
            End If

            If operation.ChangesCardStatus Then
                _log.Debug("Operation changes card status, so... changing card status to [" & operation.NewStatusId & "]")
                _cardManager.ChangeCardStatus(operation.Card.CardNumber, operation.NewStatusId)
            End If

        End Sub
    End Class
End NameSpace