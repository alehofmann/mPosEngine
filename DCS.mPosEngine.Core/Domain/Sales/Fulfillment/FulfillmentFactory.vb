'Imports DCS.PlaycardBase.Core.CardDomain
'Imports DCS.PlaycardBase.Core.DataInterfaces

'Namespace Domain.Sales.Fulfillment
'    Public Class FulfillmentFactory
'        Private ReadOnly _cardManager As ICardManager

'        Public Sub New(ByVal cardManager As ICardManager)
'            _cardManager = cardManager
'        End Sub

'        Public Function GetFulfillmentData(ByVal mPosTransaction As MPosTransaction) As OperationFulfillmentData
'            Dim retVal As New OperationFulfillmentData
'            Dim cardOperation As CardOperation

'            For Each li As MPosOperation In mPosTransaction.Operations
'                If li.IsCardRelated Then
'                    'ChargeProductToCard(li.CardNumber, li.Product.ProductData, li.Quantity)
'                    'cardOperation=New CardOperation(New CardInfo(li.CardNumber,_cardManager.IsCardSold(li.CardNumber)),
'                    AddCardOperation(retVal, li.CardNumber, li.Product.ProductData, li.Quantity)
'                End If

'            Next

'            Return retVal
'        End Function

'        Private Sub AddCardOperation(fulFillmentData As OperationFulfillmentData, ByVal cardNumber As Long, ByVal product As DCS.PlaycardBase.Core.PosDomain.Product, ByVal quantity As Decimal)
'            'Probablemente, al igual que en el Kiosk, en lguar de "product" a esta altura tendria que pasarle un "Script" de lo que tiene que hacerle a la tarjeta. Ese script
'            'En otro lugar es generado a partir del producto y puede tomar un monton de reglas en cuenta para generarlo. Al pasar aqui lisa y llanamente el producto lo que 
'            'voy a hacer es cargar simple y literalmente lo que el producto indica, ya que esta es la responsabilidad de esta rutina y clase.

'            Dim cardOperation As CardOperation
'            Dim operationType As CardOperation.CardOperationTypesEnum
'            Dim counterTransaction As CounterTransaction

'            If Not quantity > 0 Then
'                Throw New ArgumentException("quantity must be grater than zero", "quantity")
'            End If

'            If product.SellsNewCard And quantity > 1 Then
'                Throw New CannotAddMoreThanOneNewCardProductToLineitemException
'            End If

'            If product.SellsNewCard Then
'                operationType = Fulfillment.CardOperation.CardOperationTypesEnum.CardSell
'            Else
'                operationType = Fulfillment.CardOperation.CardOperationTypesEnum.CardRecharge
'            End If

'            counterTransaction = GetCounterTransaction(cardNumber, product, quantity)
'            cardOperation = New CardOperation(New CardInfo(cardNumber, _cardManager.IsCardSold(cardNumber)), operationType, product.StatusToAdd, product, counterTransaction)
'            fulFillmentData.AddCardOperation(cardOperation)

'        End Sub

'        Private Function GetCounterTransaction(ByVal cardNumber As Long, ByVal product As DCS.PlaycardBase.Core.PosDomain.Product, ByVal quantity As Decimal) As CounterTransaction
'            Dim retVal As CounterTransaction = Nothing

'            If Not product.CounterMovements Is Nothing AndAlso product.CounterMovements.Count > 0 Then
'                retVal = New CounterTransaction

'                For Each counterMovement As DCS.PlaycardBase.Core.PosDomain.ProductCounterMovement In product.CounterMovements
'                    retVal.AddItem(cardNumber, counterMovement.Amount * quantity, counterMovement.CounterType)
'                Next
'            End If


'            Return retVal


'        End Function
'    End Class
'End Namespace
