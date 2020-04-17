Imports DCS.mPosEngine.Core.Domain.Sales
Imports DCS.mPosEngine.Services.Dto
Imports NUnit.Framework
Imports DCS.mPosEngine.Services

Namespace Services
    <TestFixture()>
    Public Class TestPurchasingService
        Private _purchasingServices As New PurchasingServices(Nothing)

        <Test()>
        Public Sub TestGetTransactionInfo()
            log4net.Config.XmlConfigurator.Configure()

            Dim cart As New TransactionCartDto
            Dim li As LineitemDto
            Dim ti As TransactionInfoDto
            Dim discounts As String

            li = New LineitemDto
            li.CardNumber = 300000
            li.ProductId = 2
            li.Quantity = 1
            li.UnitPrice = 1

            cart.LineItems.Add(li)

            li = New LineitemDto
            li.CardNumber = 300000
            li.ProductId = 1
            li.Quantity = 100
            li.UnitPrice = 1

            cart.LineItems.Add(li)

            discounts = "1,2" 'Ids separados por coma (,)

            ti = _purchasingServices.GetTransactionInfo(cart, discounts, "POSTEST")


        End Sub

        <Test()>
        Public Sub TestCommitTransaction2()
            'Dim command As CommitTransactionCommandDto

            'command=helpers.GetCommitTransactionCommand(1234,3,1,10,1)

        End Sub
        <Test()>
        Public Sub TestCommitTransaction()

            Dim cart As New TransactionCartDto
            Dim li As LineitemDto
            Dim ti As TransactionInfoDto
            Dim command As New CommitTransactionCommandDto
            Dim response As CommitTransactionResponseDto
            Dim discounts As String

            log4net.Config.XmlConfigurator.Configure()

            SellAndChargeCard(1231231, 10000)

            li = New LineitemDto
            li.CardNumber = 1231231
            li.ProductId = 3
            li.Quantity = 1
            li.UnitPrice = 25

            cart.LineItems.Add(li)

            'discounts = "1,2" 'Ids separados por coma (,)
            discounts =""
            ti = _purchasingServices.GetTransactionInfo(cart, discounts, "POSTEST")

            command.Cart = cart
            command.PosName = "VERUGAZO"
            command.InvoiceNumber = ti.InvoiceNumber
            command.OperatorId = 1
            command.DiscountsApplied = discounts

            '   CASH
            'command.Payitems.Add(New PayItemDto(1, ti.TotalToPay, 0, "", "", "", ""))
            '   VOUCHERS
            command.Vouchers = GenerateEncryptedVoucher("00000001", "05") + "," + GenerateEncryptedVoucher("00000002", "05")

            response = _purchasingServices.CommitTransaction(command)

            Assert.IsTrue(response.ResultCode = CommitTransactionResponseDto.ResultCodesEnum.CommitSuccess)

        End Sub

        <Test()>
        Public Sub TestAuthorizePaymentNotEnoughCredits()
            Dim testPayment As IList(Of PayItemDto) = New List(Of PayItemDto)

            log4net.Config.XmlConfigurator.Configure()

            SellAndChargeCard(1231231, 10000)
            testPayment.Add(New PayItemDto(3, 30000, 1231231, "", "", "", ""))

            Assert.IsTrue(_purchasingServices.AuthorizePayment(testPayment).ResultCode = AuthorizePaymentResponseDto.ResultCodesEnum.NotEnoughCreditsOnPlaycard)
        End Sub

        <Test()>
        Public Sub TestAuthorizePaymentNotSoldCard()
            Dim cardManager As New Data.Infrastructure.PlaycardBaseCardManager
            Dim testPayment As IList(Of PayItemDto) = New List(Of PayItemDto)

            log4net.Config.XmlConfigurator.Configure()

            cardManager.WipeCard(1231231)
            testPayment.Add(New PayItemDto(3, 30000, 1231231, "", "", "", ""))

            Assert.IsTrue(_purchasingServices.AuthorizePayment(testPayment).ResultCode = AuthorizePaymentResponseDto.ResultCodesEnum.UnsoldPlaycard)
        End Sub

        <Test()>
        Public Sub TestAuthorizePaymentSuccess()
            Dim testPayment As IList(Of PayItemDto) = New List(Of PayItemDto)

            log4net.Config.XmlConfigurator.Configure()

            SellAndChargeCard(1231231, 10000)
            testPayment.Add(New PayItemDto(3, 10000, 1231231, "", "", "", ""))

            Assert.IsTrue(_purchasingServices.AuthorizePayment(testPayment).ResultCode = AuthorizePaymentResponseDto.ResultCodesEnum.PaymentAuthorized)
        End Sub

        Private Sub SellAndChargeCard(ByVal cardNumber As Long, ByVal creditsAmount As Decimal)
            Dim cardManager As New Data.Infrastructure.PlaycardBaseCardManager

            cardManager.WipeCard(cardNumber)
            cardManager.SellCard(cardNumber)
            cardManager.ChargeCard(cardNumber, 1, creditsAmount)

            Assert.IsTrue(cardManager.IsCardSold(cardNumber))
            Assert.IsTrue(cardManager.GetCardCounterAmount(cardNumber, 1) = creditsAmount)
        End Sub

        <Test()>
        Public Sub TestVoucherDecrypted()
            Dim voucherEncrypted = GenerateEncryptedVoucher(37, "04")

            Dim res = _purchasingServices.CheckVoucher(voucherEncrypted)

            Assert.IsTrue(res.Amount > 0 And Not res.Redeemed)
        End Sub

        Private Function GenerateEncryptedVoucher(voucherId As String, voucherType As String) As String
            Dim voucherEncrypted = voucherType + _purchasingServices.EncryptVoucherOnlyForTest(voucherId)

            Return voucherEncrypted
        End Function

        <Test()>
        Public Sub TestRedeemVouchers()
            Dim vouchers As String

            vouchers = GenerateEncryptedVoucher("00000001", "05") + ","
            vouchers = vouchers + GenerateEncryptedVoucher("00000002", "05")

            _purchasingServices.RedeemVouchers(vouchers, 1, "mPosTest")

            'Assert.IsTrue(res)
        End Sub
    End Class
End Namespace