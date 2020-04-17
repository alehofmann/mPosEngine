Imports System.IO
Imports System.Runtime.Serialization.Json
Imports System.ServiceModel

Imports DCS.mPosEngine.Services
Imports DCS.mPosEngine.Services.Dto
Imports DCS.mPosEngine.Services.Payment
Imports DCS.mPosEngine.WebService.CommObjects

<ServiceBehavior(ConcurrencyMode:=ConcurrencyMode.Single, InstanceContextMode:=ServiceModel.InstanceContextMode.Single)> _
Public Class ServiceHost
    Implements IServiceHost

	Private _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private ReadOnly _productServices As ProductServices
    Private ReadOnly _cardServices As CardServices
    Private ReadOnly _posServices As PosServices
	Private ReadOnly _purchasingServices As PurchasingServices
	Private ReadOnly _treasuryServices As TreasuryServices
    Private ReadOnly _paymentServices As PaymentService

	Public Sub New()
		Try
			_log.Info("Creating: Product Services")
			_productServices = New ProductServices
			_log.Info("Creating: Card Services")
			_cardServices = New CardServices
			_log.Info("Creating: Pos Services")
			_posServices = New PosServices
            _log.Info("Creating: Payment Services")
			_paymentServices = New PaymentService()
			_log.Info("Creating: Purchasing Services")
			_purchasingServices = New PurchasingServices(_paymentServices)
			_log.Info("Creating: Treasury Services")
			_treasuryServices = New TreasuryServices()
		Catch ex As Exception
			_log.Error("Error at initialize Web Service: " + ex.Message, ex)
			_log.Info("Stopping service")
			Throw
		End Try
	End Sub

	Public Function GetProducts(ByVal userCardNumber As Integer) As CommandResponse(Of IList(Of ProductDto)) Implements IServiceHost.GetProducts
        Dim retVal As New CommandResponse(Of IList(Of ProductDto))

        _log.Info("Command Recieved: GetProducts (userCardnumber=" & userCardNumber & ")")
        Try
            retVal.Body = _productServices.GetProducts(userCardNumber)
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try
        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

		Return retVal
	End Function

    Public Function AnalyzeCard(ByVal cardNumber As Integer) As CommandResponse(Of CardDataDto) Implements IServiceHost.AnalyzeCard
        Dim retVal As New CommandResponse(Of CardDataDto)

        _log.Info("Command Recieved: AnalyzeCard (cardNumber=" & cardNumber & ")")
        Try
            retVal.Body = _cardServices.CardAnalyze(cardNumber)
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try
        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

		Return retVal
	End Function

    Public Function GetPaymodes() As CommandResponse(Of IList(Of PaymodeDto)) Implements IServiceHost.GetPaymodes
        Dim retVal As New CommandResponse(Of IList(Of PaymodeDto))

        _log.Info("Command Recieved: GetPaymodes")

        Try
            retVal.Body = _posServices.GetPaymodes
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try

        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

        Return retVal
    End Function

    Public Function GetCardHistory(ByVal cardNumber As Integer, ByVal maxLines As Integer) As CommandResponse(Of IList(Of CardHistoryLineDto)) Implements IServiceHost.GetCardHistory
        Dim retVal As New CommandResponse(Of IList(Of CardHistoryLineDto))

        _log.Info("Command Recieved: GetCardHistory (cardNumber=" & cardNumber & ", maxLines=" & maxLines & ")")

        Try
            retVal.Body = _cardServices.GetCardHistory(cardNumber, maxLines)
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try

        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

        Return retVal
    End Function

    Public Function LoginCashier(ByVal loginMode As Integer, ByVal cardNumber As Integer, ByVal userId As String, ByVal password As String) As CommandResponse(Of LoginDataDto) Implements IServiceHost.LoginCashier
        Dim retVal As New CommandResponse(Of LoginDataDto)

        _log.Info("Command Recieved: LoginCashier (loginMode=" & loginMode & ", cardNumber=" & cardNumber & ", userId=" & userId & ", password=*********)")

        Try
            retVal.Body = _posServices.LoginCashier(loginMode, cardNumber)
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try

        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

        Return retVal
    End Function

    Public Function GetPosConfig(posName As String) As CommandResponse(Of IList(Of PosConfigDto)) Implements IServiceHost.GetPosConfig
        Dim retVal As New CommandResponse(Of IList(Of PosConfigDto))

        _log.Info("Command Recieved: GetPosConfig (posName=" & posName & ")")

        Try
            retVal.Body = _posServices.GetPosConfig(posName)
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try

        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

        Return retVal
    End Function

    Public Function CheckDevice(deviceSerial As String) As CommandResponse(Of CheckDeviceResponseDto) Implements IServiceHost.CheckDevice
        Dim retVal As New CommandResponse(Of CheckDeviceResponseDto)

        _log.Info("Command Recieved: CheckDevice (deviceSerial=" & deviceSerial & ")")

        Try
            retVal.Body = _posServices.CheckDevice(deviceSerial)
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try

        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

        Return retVal
    End Function

    Public Function CheckAccess(actionId As Integer, userCardNumber As Integer) As CommandResponse(Of CheckAccessResponseDto) Implements IServiceHost.CheckAccess
        Dim retVal As New CommandResponse(Of CheckAccessResponseDto)

        _log.Info("Command Recieved: CheckAccess (actionId=" & actionId & ", userCardNumber=" & userCardNumber & ")")

        Try
            retVal.Body = _posServices.CheckAccess(actionId, userCardNumber)
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try

        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

        Return retVal
    End Function

    Private Function GetErrorDescription(ByVal ex As Exception) As String
        'If ex.GetType Is GetType(ApplicationException) Then
        'Return ex.Message
        'Else
        'Return "Unhandled error, please read mPosEngine log"
        'End If
        Return ex.ToString
    End Function

	Public Function GetAdminFunctions(userCardNumber As Integer) As CommObjects.CommandResponse(Of System.Collections.Generic.IList(Of Services.Dto.AdminFunctionDto)) Implements IServiceHost.GetAdminFunctions
		Dim retVal As New CommandResponse(Of IList(Of AdminFunctionDto))

		_log.Info("Command Recieved: GetAdminFunctions (userCardNumber=" & userCardNumber & ")")

		Try
			retVal.Body = _posServices.GetAdminFunctions(userCardNumber)
			retVal.Header.CommandSuccess = True
		Catch ex As Exception
			retVal.Header.CommandSuccess = False
			retVal.Header.ErrorDescription = GetErrorDescription(ex)
		End Try

		_log.Info("Sending command response")
		_log.Debug(retVal.ToString)

		Return retVal
	End Function

	Public Function RegisterDevice(deviceSerial As String, pairCode As String) As CommObjects.CommandResponse(Of Services.Dto.RegisterDeviceResponseDto) Implements IServiceHost.RegisterDevice
        Dim retVal As New CommandResponse(Of RegisterDeviceResponseDto)

        _log.Info("Command Recieved: RegisterDevice (deviceSerial=" & deviceSerial & ", pairCode=" & pairCode & ")")

        Try
            retVal.Body = _posServices.RegisterDevice(deviceSerial, pairCode)
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try

        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

		Return retVal
	End Function

	Public Function GetTransactionInfo(transactionCart As TransactionCartDto, discountsApplied As String, posName As String) As CommObjects.CommandResponse(Of Services.Dto.TransactionInfoDto) Implements IServiceHost.GetTransactionInfo
		Dim retVal As New CommandResponse(Of TransactionInfoDto)

		_log.Info("Command Recieved: GetTransactionInfo")
		_log.Debug(ToJson(Of TransactionCartDto)(transactionCart))

		Try
			retVal.Body = _purchasingServices.GetTransactionInfo(transactionCart, discountsApplied, posName)
			retVal.Header.CommandSuccess = True
		Catch ex As Exception
			retVal.Header.CommandSuccess = False
			retVal.Header.ErrorDescription = GetErrorDescription(ex)
		End Try

		_log.Info("Sending command response")
		_log.Debug(retVal.ToString)

		Return retVal
	End Function

	Public Function GetCardStatus(cardNumber As Integer) As CommObjects.CommandResponse(Of Services.Dto.CardStatusDto) Implements IServiceHost.GetCardStatus
        Dim retVal As New CommandResponse(Of CardStatusDto)

        _log.Info("Command Recieved: GetCardStatus (cardNumber=" & cardNumber & ")")

        Try
            retVal.Body = _cardServices.GetCardStatus(cardNumber)
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try

        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

		Return retVal
	End Function

	Public Function ToJson(Of T)(ByVal source As T)
		Dim js As New DataContractJsonSerializer(GetType(T))
		Dim ms As New MemoryStream

		js.WriteObject(ms, source)

		ms.Position = 0
		Return (New StreamReader(ms)).ReadToEnd
	End Function

	Public Function CommitTransaction(command As Services.Dto.CommitTransactionCommandDto) As CommObjects.CommandResponse(Of Services.Dto.CommitTransactionResponseDto) Implements IServiceHost.CommitTransaction
        Dim retVal As New CommandResponse(Of CommitTransactionResponseDto)

        _log.Info("Command Recieved: CommitTransaction")
        _log.Debug(ToJson(Of CommitTransactionCommandDto)(command))

        Try
            retVal.Body = _purchasingServices.CommitTransaction(command)
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try

        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

        Return retVal
    End Function

	Public Function CardTransfer(command As Services.Dto.CardTransferCommandDto) As CommObjects.CommandResponse(Of Services.Dto.CardTransferResponseDto) Implements IServiceHost.CardTransfer
		Dim retVal As New CommandResponse(Of CardTransferResponseDto)

		_log.Info("Command Recieved: CardTransfer")
		_log.Debug(ToJson(Of CardTransferCommandDto)(command))
		Try
			retVal.Body = _cardServices.CardTransfer(command)
			retVal.Header.CommandSuccess = True
		Catch ex As Exception
			retVal.Header.CommandSuccess = False
			retVal.Header.ErrorDescription = GetErrorDescription(ex)
		End Try

		_log.Info("Sending command response")
		_log.Debug(retVal.ToString)

		Return retVal
	End Function

	Public Function CardConsolidate(command As Services.Dto.CardConsolidateCommandDto) As CommObjects.CommandResponse(Of Services.Dto.CardTransferResponseDto) Implements IServiceHost.CardConsolidate
        Dim retVal As New CommandResponse(Of CardTransferResponseDto)

        _log.Info("Command Recieved: CardConsolidate")
        _log.Debug(ToJson(Of CardConsolidateCommandDto)(command))

        Try
            retVal.Body = _cardServices.CardConsolidate(command)
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try

        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

        Return retVal
    End Function

    Public Function AuthorizePayment(paymentData As System.Collections.Generic.IList(Of Services.Dto.PayItemDto)) As CommObjects.CommandResponse(Of Services.Dto.AuthorizePaymentResponseDto) Implements IServiceHost.AuthorizePayment
        Dim retVal As New CommandResponse(Of AuthorizePaymentResponseDto)

        _log.Info("Command Recieved: AuthorizePayment")
        _log.Debug(ToJson(Of IList(Of PayItemDto))(paymentData))

        Try
            retVal.Body = _purchasingServices.AuthorizePayment(paymentData)
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try

        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

        Return retVal
    End Function

	Public Function GetProductPages(posId As String, userCardNumber As Integer) As CommObjects.CommandResponse(Of GetProductPagesResponseDto) Implements IServiceHost.GetProductPages
		Dim retVal As New CommandResponse(Of GetProductPagesResponseDto)

		_log.Info("Command Recieved: GetProductPages")
		_log.Debug("posId=" & posId & ", userCardNumber=" & userCardNumber)

		Try
			retVal.Body = _productServices.GetProductPages(posId, userCardNumber)
			retVal.Header.CommandSuccess = True
		Catch ex As Exception
			retVal.Header.CommandSuccess = False
			retVal.Header.ErrorDescription = GetErrorDescription(ex)
		End Try

		_log.Info("Sending command response")
		_log.Debug(retVal.ToString)

		Return retVal
	End Function

	Public Function GetDiscountsFromProducts(transactionCart As TransactionCartDto) As CommandResponse(Of List(Of DiscountDto)) Implements IServiceHost.GetDiscountsFromProducts
		Dim retVal As New CommandResponse(Of List(Of DiscountDto))

		_log.Info("Command Recieved: GetDiscountsFromProducts")
		_log.Debug(ToJson(Of TransactionCartDto)(transactionCart))

		Try
			retVal.Body = _purchasingServices.GetDiscountsFromProducts(transactionCart)
			retVal.Header.CommandSuccess = True
		Catch ex As Exception
			retVal.Header.CommandSuccess = False
			retVal.Header.ErrorDescription = GetErrorDescription(ex)
		End Try

		_log.Info("Sending command response")
		_log.Debug(retVal.ToString)

		Return retVal
	End Function

	Public Function CashPull(cashierId As Integer, cash As List(Of MoneyOperationDto)) As CommandResponse(Of SuccessResponse) Implements IServiceHost.CashPull
		Dim retVal As New CommandResponse(Of SuccessResponse)

		_log.Info("Command Recieved: CashPull")
		_log.Debug("CashierId: " + cashierId.ToString())
		_log.Debug(ToJson(cash))

		Try
			retVal.Body = New SuccessResponse(_treasuryServices.CashPull(cashierId, cash))
			retVal.Header.CommandSuccess = True
		Catch ex As Exception
			retVal.Header.CommandSuccess = False
			retVal.Header.ErrorDescription = GetErrorDescription(ex)
		End Try

		_log.Info("Sending command response")
		_log.Debug(retVal.ToString)

		Return retVal
	End Function

	Public Function PayOut(cashierId As Integer, reason As String, cash As List(Of MoneyOperationDto)) As CommandResponse(Of SuccessResponse) Implements IServiceHost.PayOut
		Dim retVal As New CommandResponse(Of SuccessResponse)

		_log.Info("Command Recieved: PayOut")
		_log.Debug("CashierId: " + cashierId.ToString())
		_log.Debug("Reason: " + reason)
		_log.Debug(ToJson(cash))

		Try
			retVal.Body = New SuccessResponse(_treasuryServices.PayOut(cashierId, reason, cash))
			retVal.Header.CommandSuccess = True
		Catch ex As Exception
			retVal.Header.CommandSuccess = False
			retVal.Header.ErrorDescription = GetErrorDescription(ex)
		End Try

		_log.Info("Sending command response")
		_log.Debug(retVal.ToString)

		Return retVal
	End Function

	Public Function GetBalance(cashierId As Integer) As CommandResponse(Of TreasuryBalanceDto) Implements IServiceHost.GetBalance
		Dim retVal As New CommandResponse(Of TreasuryBalanceDto)

		_log.Info("Command Recieved: GetBalance")
		_log.Debug("CashierId: " + cashierId.ToString())

		Try
			retVal.Body = _treasuryServices.GetBalance(cashierId)
			retVal.Header.CommandSuccess = True
		Catch ex As Exception
			retVal.Header.CommandSuccess = False
			retVal.Header.ErrorDescription = GetErrorDescription(ex)
		End Try

		_log.Info("Sending command response")
		_log.Debug(retVal.ToString)

		Return retVal
	End Function

    public Function GetCreditCardTransactionStatus(logCreditCardTransactionId As integer) As CommandResponse(Of CreditCardStatusDto) Implements IServiceHost.GetCreditCardTransactionStatus
        Dim retVal As New CommandResponse(Of CreditCardStatusDto)

		_log.Info("Command Recieved: GetCreditCardTransactionStatus")
		_log.Debug("TransactionId: " + logCreditCardTransactionId.ToString())

		Try
			retVal.Body = _paymentServices.GetTransactionStatus(logCreditCardTransactionId)
			retVal.Header.CommandSuccess = True
		Catch ex As Exception
			retVal.Header.CommandSuccess = False
			retVal.Header.ErrorDescription = GetErrorDescription(ex)
		End Try

		_log.Info("Sending command response")
		_log.Debug(retVal.ToString)

		Return retVal
    End Function

    Public Function ReturnCreditCardTransaction(invoiceId As Long, logCreditCardTransactionId As Integer, purchaseAmount As Decimal) As CommandResponse(Of ReturnCreditCardResponseDto) Implements IServiceHost.ReturnCreditCardTransaction
        Dim retVal As New CommandResponse(Of ReturnCreditCardResponseDto)

        _log.Info("Command Recieved: ReturnCreditCardTransaction")
        _log.Debug("TransactionId: " + logCreditCardTransactionId.ToString)
        _log.Debug("Amount: " + purchaseAmount.ToString)

        Try
            retVal.Body = _paymentServices.ReturnCreditCardTransaction(invoiceId, logCreditCardTransactionId, purchaseAmount)
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try

        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

        Return retVal
    End Function

    Public Function StartCreditCardEngine(provider As String, config As ilist(Of KeyValuePair(Of String, String))) As CommandResponse(Of GenericResultResponseDto) Implements IServiceHost.StartCreditCardAuthorizer
        Dim retVal As New CommandResponse(Of GenericResultResponseDto)

        _log.Info("Command received: StartCreditCardEngine")
        _log.Debug("Provider: " + provider)
        _log.Debug("Config count: " & If(config IsNot Nothing, config.Count, 0))

        Try
            retVal.Body = _paymentServices.StartCreditCardAuthorizer(provider, config)
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try

        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

        Return retVal
    End Function

    Public Function CheckVoucher(voucherCode As String) As CommandResponse(Of VoucherResponseDto) Implements IServiceHost.CheckVoucher
        Dim retVal As New CommandResponse(Of VoucherResponseDto)

        _log.Info("Command received: CheckVoucher")
        _log.Debug("VoucherCode: " + voucherCode)

        Try
            retVal.Body = _purchasingServices.CheckVoucher(voucherCode)
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try

        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

        Return retVal
    End Function

    Public Function GetInvoiceImage(filename As String) As CommandResponse(Of InvoiceImageResponseDto) Implements IServiceHost.GetInvoiceImage
        Dim retVal As New CommandResponse(Of InvoiceImageResponseDto)

        _log.Info("Command received: GetInvoiceImage")
        _log.Debug("Filename: " + filename)

        Try
            retVal.Body = _posServices.GetInvoiceImage(filename)
            retVal.Header.CommandSuccess = True
        Catch ex As Exception
            retVal.Header.CommandSuccess = False
            retVal.Header.ErrorDescription = GetErrorDescription(ex)
        End Try

        _log.Info("Sending command response")
        _log.Debug(retVal.ToString)

        Return retVal
    End Function
End Class