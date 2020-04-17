Imports DCS.PlaycardBase.Core.CardDomain
Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment

Namespace Domain.Sales
    Public Class ProductReturnOperation
        Inherits MPosOperation

		Public Sub New()
		End Sub

		Friend Sub New(ByVal priceSettled As Decimal, ByVal quantity As Decimal, ByVal product As DCS.PlaycardBase.Core.PosDomain.Product, ByVal taxExemptTransaction As Boolean, Optional ByVal cardInfo As CardInfo = Nothing)
            MyBase.New(priceSettled, quantity, product, taxExemptTransaction, cardInfo)

			'Retrieve/Calculate fulfillment data**************************
			Dim operationType As CardOperation.CardOperationTypesEnum
            FulfillmentData = New OperationFulfillmentData

            If IsCardRelated Then
                If product.SellsNewCard Then
                    FulfillmentData.CardOperation = New CardOperation(cardInfo, operationType = CardOperation.CardOperationTypesEnum.CardReturn, 0, GetCounterTransaction(cardInfo.CardNumber, product, quantity))
                Else
                    FulfillmentData.CardOperation = New CardOperation(cardInfo, operationType = operationType = CardOperation.CardOperationTypesEnum.CardUncharge, Nothing, GetCounterTransaction(cardInfo.CardNumber, product, quantity))
                End If

            End If
            '*************************************************************
        End Sub

        Private Function GetCounterTransaction(ByVal cardNumber As Long, ByVal productData As DCS.PlaycardBase.Core.PosDomain.Product, ByVal productQuantity As Decimal) As CounterTransaction
            Dim retVal As CounterTransaction = Nothing

            If Not productData.CounterMovements Is Nothing AndAlso productData.CounterMovements.Count > 0 Then
                retVal = New CounterTransaction

                For Each counterMovement As DCS.PlaycardBase.Core.PosDomain.ProductCounterMovement In Product.CounterMovements
                    'retVal.AddItem(cardNumber, counterMovement.Amount * productQuantity, counterMovement.CounterType)
                    retVal.AddItem(cardNumber, counterMovement.GetConvertedAmount(productQuantity), counterMovement.CounterType)
                Next
            End If


            Return retVal
        End Function

        Public Overrides ReadOnly Property OperationType As Byte
            Get
                Return 2
            End Get
        End Property

        Public Overrides ReadOnly Property SellsNewCard As Boolean
            Get
                Return False
            End Get
        End Property
    End Class
End Namespace
