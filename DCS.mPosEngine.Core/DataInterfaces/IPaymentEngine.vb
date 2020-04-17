Namespace DataInterfaces
    Public Interface IPaymentEngine
        Enum AuthorizeStatusEnum
            PaymentAuthorized = 0
            NotEnoughPlaycardBalance = 1
            UnsoldPlaycard = 2
        End Enum

        Function AuthorizePayment(ByVal payItem As mPosEngine.Core.Domain.Sales.Payment.PayItem, ByVal onlyAuthorize As Boolean) As AuthorizeStatusEnum

    End Interface
End Namespace
