Imports DCS.PlaycardBase.Core.CardDomain
Imports DCS.mPosEngine.Core.Domain.Sales
Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment
Imports DCS.mPosEngine.Core.Domain.ExternalSales

Namespace Domain.Sales
    Public Class ProductSellOperation
        Inherits MPosOperation

		Private _discountAmount As Decimal = 0
		Private _discountApplied As Boolean = False
        Private _externalSystemInfo As ExternalSystem

        Private Sub New()

        End Sub
        Public ReadOnly Property CommitToExternalSystem As Boolean
            Get
                Return _externalSystemInfo IsNot Nothing
            End Get
        End Property

        Public Property ExternalSystemInfo() As ExternalSystem
            Get
                Return _externalSystemInfo
            End Get
            Set(ByVal value As ExternalSystem)
                _externalSystemInfo = value
            End Set
        End Property

        Friend Sub New(ByVal priceSettled As Decimal, ByVal quantity As Decimal, ByVal product As DCS.PlaycardBase.Core.PosDomain.Product, ByVal taxExemptTransaction As Boolean, Optional ByVal cardInfo As CardInfo = Nothing, Optional ByVal externalSystemInfo As ExternalSystem = Nothing)
            MyBase.New(priceSettled, quantity, product, taxExemptTransaction, cardInfo)

            _externalSystemInfo = externalSystemInfo

            'Retrieve/Calculate fulfillment data**************************
            Dim operationType As CardOperation.CardOperationTypesEnum
            FulfillmentData = New OperationFulfillmentData

            If IsCardRelated Then
                If product.SellsNewCard Then
                    operationType = CardOperation.CardOperationTypesEnum.CardSell
                Else
                    operationType = CardOperation.CardOperationTypesEnum.CardRecharge
                End If
                FulfillmentData.CardOperation = New CardOperation(cardInfo, operationType, product.StatusToAdd, GetCounterTransaction(cardInfo.CardNumber, product, quantity))
            End If
            '*************************************************************
        End Sub

		'Solo coherente para un ProductSell
		Public Sub ApplyDiscount()
            If Fulfilled Then
                Throw New InvalidOperationException("Operation already fulfilled")
            End If

            If _discountApplied Then
                Throw New InvalidOperationException("Discount already applied to this operation")
            End If

            'If _taxApplied Then
            'Throw New InvalidOperationException("Discount must be applied BEFORE taxes")
            'End If
        End Sub

        Private Function GetCounterTransaction(ByVal cardNumber As Long, ByVal productData As DCS.PlaycardBase.Core.PosDomain.Product, ByVal productQuantity As Decimal) As CounterTransaction
            Dim retVal As CounterTransaction = Nothing

            If Not productData.CounterMovements Is Nothing AndAlso productData.CounterMovements.Count > 0 Then
                retVal = New CounterTransaction

                For Each counterMovement As DCS.PlaycardBase.Core.PosDomain.ProductCounterMovement In Product.CounterMovements
                    'Deberia tomar el GetConvertedAmount
                    'retVal.AddItem(cardNumber, counterMovement.Amount * productQuantity, counterMovement.CounterType)
                    retVal.AddItem(cardNumber, counterMovement.GetConvertedAmount(productQuantity), counterMovement.CounterType)
                Next
            End If


            Return retVal


        End Function

        Public Overrides ReadOnly Property OperationType As Byte
            Get
                Return 1
            End Get
        End Property

        Public Overrides ReadOnly Property SellsNewCard As Boolean
            Get
                Return (FulfillmentData.CardOperation IsNot Nothing AndAlso FulfillmentData.CardOperation.SellsNewCard)
            End Get
        End Property
    End Class
End Namespace