Imports System
Imports System.ComponentModel
Imports System.Timers
Imports CCard

Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.mPosEngine.Core.Domain.Sales.Payment
Imports DCS.mPosEngine.Data.Dao
Imports DCS.mPosEngine.Data.Infrastructure
Imports DCS.mPosEngine.Services.Dto
Imports DCS.PlaycardBase.Core.CreditCardDomain
Imports DCS.PlaycardBase.Core.DataInterfaces
Imports DCS.PlaycardBase.Core.PosDomain
Imports DCS.PlaycardBase.Data
Imports Timer = System.Timers.Timer

Namespace Payment
    Public Class PaymentDeclinedException
        Inherits ApplicationException
    End Class

    Public Class PaymentService
        Private Shared _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Private ReadOnly _playcardPaymentEngine As PlaycardPaymentEngine
        Private _ccEngine As CCard.DCSAuthorizationEngine
        Private _ccProvider As string

        Private WithEvents _bgWorker as BackgroundWorker
        Private _bgWorkerIsRunning as Boolean
        Private WithEvents _timerBgWorker As System.Timers.Timer
        Private Const TimerTimeout As Integer = 180000

        Public Enum PaymentResultEnum
            PaymentSuccess = 1
            NotEnoughPlaycardBalance = 2
            UnsoldPlaycard = 3
            PaymentProcessed = 4
            PaymentUndefinedError = 5
        End Enum

        public enum CcardTransactionStatus
            Processing = 1
            [Error] = 2
            Ok = 3
            Declined = 4
        End enum

        Public Sub New()
            _playcardPaymentEngine = New PlaycardPaymentEngine
        End Sub

        public Property CreditCardEngineStarted() As Boolean

        public Function StartCreditCardAuthorizer(provider As String, config As ilist(Of KeyValuePair(Of string, string))) As GenericResultResponseDto
            'dim cCardConnString = "PROVIDER:=TranCloud;TERMINALTYPE:=1;PINPADIP:=192.168.3.103;PINPADPORT:=12000;SECUREDEVICE:=CloudEMV2;OPERATORID:=Test"

            If Me.CreditCardEngineStarted then
                _log.Error("Credit Card engine already started")
                Return new GenericResultResponseDto(1, "Credit Card engine already started")
            End If

            dim cCardConnString = "PROVIDER:=" + provider + ";"

            For each val as KeyValuePair(Of string, string) In config
                cCardConnString += val.Key.ToUpper() + ":=" + val.Value + ";"
            Next

            try
                _log.Debug("Instancing DCSAuthorizationEngine")
                _log.Debug("ConnectionString: " + cCardConnString)

                _ccEngine = new CCard.DCSAuthorizationEngine

                _log.Debug("Creating DCSAuthorizationEngine")

                _ccengine.Create(cCardConnString, enuLogModes.lmDCSFile)

                If Not _ccengine.Ready() Then
                    _log.Error("Can't create Ccard Engine. Error: " + _ccengine.LastError)
                    return New GenericResultResponseDto(2, "Can't create Ccard Engine. Error: " + _ccengine.LastError)
                End If

                _ccProvider = provider
            Catch ex As Exception
                _log.Error("Can't create CcardAuthorizer. Error: " + ex.Message, ex)
                throw
            end try

            Me.CreditCardEngineStarted = True

            Return new GenericResultResponseDto(0, "Credit Card Engine created succesfully")
        End Function

        Public Function AuthorizePayment(ByVal paymentData As PaymentData, byref transactionId As integer) As PaymentResultEnum
            Dim result As IPaymentEngine.AuthorizeStatusEnum

            For Each payItem As PayItem In paymentData.PayItems
                Select Case payItem.Currency.PaymodeType
                    Case Currency.PaymodeTypesEnum.PlaycardPaymodeType
                        _log.Debug("Validating Playcard payment")
                        result = _playcardPaymentEngine.AuthorizePayment(payItem, True)

                        Select Case result
                            Case Core.DataInterfaces.IPaymentEngine.AuthorizeStatusEnum.PaymentAuthorized
                                'Return PaymentResultEnum.PaymentSuccess
                            Case Core.DataInterfaces.IPaymentEngine.AuthorizeStatusEnum.NotEnoughPlaycardBalance
                                Return PaymentResultEnum.NotEnoughPlaycardBalance
                            Case Core.DataInterfaces.IPaymentEngine.AuthorizeStatusEnum.UnsoldPlaycard
                                Return PaymentResultEnum.UnsoldPlaycard
                            Case Else
                                Throw New ApplicationException("Playcard payment engine returned unkown result: " & result)
                        End Select

                    case Currency.PaymodeTypesEnum.CreditCardPaymodeType
                        _log.Debug("Validating CreditCard payment")

                        Dim ccData as CreditCardData = cType(payItem.PaymodeData, Core.Domain.Sales.Payment.CreditCardData)

                        If ccData is nothing then
                            _log.Error("Can't get invoice number")
                            Throw new ApplicationException("Can't get invoice number")
                        End If

                        _log.Debug("Validating invoice number")

                        dim invoiceNr As String = ccData.AuthorizationReference

                        If String.IsNullOrWhiteSpace(invoiceNr)
                            _log.Error("Invoice Number can't be empty")
                            Throw new ApplicationException("Invoice Number can't be empty")
                        End If

                        result = AuthorizeCreditCardPayment(payItem.Amount, payitem.Gratuity, invoiceNr)

                        Select Case result
                            Case > 0
                                transactionId = result
                                Return PaymentResultEnum.PaymentProcessed
                            Case else
                                Return PaymentResultEnum.PaymentUndefinedError
                        End Select

                    Case Else
                        _log.Warn("Only playcard or creditcard payment can be authorized, returning 'success' for any other type")
                        'Return PaymentResultEnum.PaymentSuccess
                End Select
            Next

            Return PaymentResultEnum.PaymentSuccess
        End Function

        Public Function CommitPayment(ByVal operatorId As Integer, ByVal paymentData As PaymentData) As PaymentResultEnum
            Dim commitedPayitems As IList(Of PayItem) = New List(Of PayItem)
            Dim retVal As PaymentResultEnum

            'If transaction Is Nothing Then
            '    Throw New ArgumentNullException("transaction")
            'End If

            _log.Info("Processing CommitPayment")

            If paymentData Is Nothing Then
                Throw New ArgumentNullException("paymentData")
            End If

            retVal = PaymentResultEnum.PaymentSuccess

            _log.Debug("Checking " & paymentData.PayItems.Count & " payitems")
            For Each item As PayItem In paymentData.PayItems
                Select Case item.Currency.PaymodeType
                    Case PlaycardBase.Core.PosDomain.Currency.PaymodeTypesEnum.PlaycardPaymodeType
                        'Aca tiene que autorizar/ejecutar el pago.
                        _log.Debug("Playcard payment found, validating...")
                        Select Case _playcardPaymentEngine.AuthorizePayment(item, False)
                            Case Core.DataInterfaces.IPaymentEngine.AuthorizeStatusEnum.NotEnoughPlaycardBalance
                                _log.Warn("Payitem validation failed, not enough balance in playcard " & CType(item.PaymodeData, PlaycardData).CardNumber)
                                retVal = PaymentResultEnum.NotEnoughPlaycardBalance
                                Exit For
                            Case Core.DataInterfaces.IPaymentEngine.AuthorizeStatusEnum.PaymentAuthorized
                                _log.Debug("Payitem validation success, payment authorized")
                                retVal = PaymentResultEnum.PaymentSuccess
                            Case Core.DataInterfaces.IPaymentEngine.AuthorizeStatusEnum.UnsoldPlaycard
                                _log.Warn("Payitem validation failed, card " & CType(item.PaymodeData, PlaycardData).CardNumber & " was not sold")
                                retVal = PaymentResultEnum.UnsoldPlaycard
                                Exit For
                        End Select

                    Case PlaycardBase.Core.PosDomain.Currency.PaymodeTypesEnum.CashPaymodeType
                        'Aca tiene que "avisar" a tesorería -> NO, esto lo hace en otro lado
                        _log.Debug("Cash payitem find, no validation required")
                    Case PlaycardBase.Core.PosDomain.Currency.PaymodeTypesEnum.CreditCardPaymodeType
                        'Aca no tiene que hacer nada
                        _log.Debug("Credit Card payitem find, no validation required")
                    Case Else
                        _log.Error("Unkown paymode type: " & item.Currency.PaymodeType & " found, throwing exception")
                        Throw New ApplicationException("Unkown paymode type: " & item.Currency.PaymodeType)
                End Select

                commitedPayitems.Add(item)
            Next

            If Not retVal Then
                'Aca tendria que rollbackear lo que llego a commitear                
            Else
                _log.Info("All payitems successfully validated")
            End If

            Return retVal
        End Function

        public function GetTransactionStatus(logCreditCardTransactionId as integer) As CreditCardStatusDto
            Dim res as CreditCardStatusDto
            Dim transaction as CreditCardTransaction
            Dim ccDao As ICreditCardDao

            Try
                ccdao = New CreditCardDao

                _log.info("Getting credit card transaction")

                ccDao.Session.Clear()

                _log.Debug("Clear session done")

                transaction = ccDao.GetById(logCreditCardTransactionId)
            Catch ex As Exception
                _log.Error("Error at get transaction. Message: " + ex.Message, ex)
                throw
            End Try

            If transaction is Nothing then
                _log.Error("Transaction id " & logCreditCardTransactionId & " not exists")
                return new CreditCardStatusDto(CcardTransactionStatus.Error, -1, "Transaction " & logCreditCardTransactionId & " not exists", "")
            End If

            if Not String.IsNullOrWhiteSpace(transaction.ProcessorType) Andalso not String.IsNullOrWhiteSpace(_ccProvider) AndAlso transaction.ProcessorType.ToUpper() <> _ccProvider.ToUpper() then
                _log.Error("Transaction don't belongs to current provider")
                return new CreditCardStatusDto(CcardTransactionStatus.Error, -1, "Transaction don't belongs to current provider", "")
            End If

            Dim finCode as Integer = Convert.ToInt32(transaction.FinishCode)

            _log.Debug("FinishCode: " & finCode)

            select case finCode
                case 0
                    res = New CreditCardStatusDto(CcardTransactionStatus.Processing, finCode, "", "")
                case 1 
                    res = new CreditCardStatusDto(CcardTransactionStatus.Ok, finCode, transaction.NetepayResponse.TextResponse, transaction.NetepayResponse.PrintData, transaction.CreditCardNumber, transaction.CardType)
                case > 1
                    res = new CreditCardStatusDto(CcardTransactionStatus.Declined, finCode, transaction.NetepayResponse.TextResponse, transaction.NetepayResponse.PrintData)
                Case else
                    res = new CreditCardStatusDto(CcardTransactionStatus.Error, finCode, "Undefined error", "")
            End Select

            _log.Info("Transaction founded")
            _log.Debug("Status: " & CType(res.status, CcardTransactionStatus).ToString())
            _log.Debug("FinishCode: " & finCode)
            if not String.IsNullOrWhiteSpace(res.ErrorMessage) then _log.Debug("Response: " + res.ErrorMessage)

            return res
        End function

        public Function AuthorizeCreditCardPayment(purchaseAmount As Decimal, gratuityAmount As decimal, invoiceId As string) As integer
            dim trId As integer
            Dim transaction As New TransactionDefinition

            If _ccEngine is Nothing OrElse Not Me.CreditCardEngineStarted then
                _log.Error("Credit Card Authorizer not initialized")
                throw new ApplicationException("Credit Card Authorizer not initialized")
            End If

            transaction.amount = purchaseAmount
            transaction.Gratuity = gratuityAmount
            transaction.transactionReference = invoiceid
            transaction.transactionId = invoiceid
            transaction.TransactionType = enuTransactionType.ttSale

            try
                trid = _ccengine.CreateTransaction(transaction, _ccProvider)

                if trId <= 0 then
                    _log.Error("Can't create credit card transaction")
                    return -1
                End If

                _log.Debug("SqlTransactionId created: " & trid)

                transaction.sqlTransactionId = trid
            Catch ex As Exception
                _log.Error("Error at create credit card transaction. Message: " + ex.Message, ex)
                Return -1
            End Try

            try
                _log.Debug("Creating background worker for process payment")
                _bgWorker = new BackgroundWorker()
                _bgworker.WorkerSupportsCancellation = True
                _bgWorker.RunWorkerAsync(transaction)
                _bgWorkerIsRunning = true

                _log.Debug("Background worker created, starting timer for " & TimerTimeout & " miliseconds")

                _timerBgWorker = New Timer(TimerTimeout)
                _timerbgworker.Start()

                _log.Debug("Timer ok")
            Catch ex As Exception
                _log.Error("Error at create payment thread. Message: " + ex.Message, ex)
                Throw new ApplicationException("Error at create payment process")
            end try

            return trId
        End Function

        Public Function ReturnCreditCardTransaction(invoiceId As Long, logCreditCardTransactionId As Integer, purchaseAmount As Decimal) As ReturnCreditCardResponseDto
            If _ccEngine Is Nothing OrElse Not Me.CreditCardEngineStarted Then
                _log.Error("Credit Card Authorizer not initialized")
                Throw New ApplicationException("Credit Card Authorizer not initialized")
            End If

            If purchaseAmount >= 0 Then
                _log.Error("Amount can't be zero o positive (" & purchaseAmount & ")")
                Throw New ApplicationException("Amount can't be zero o positive (" & purchaseAmount & ")")
            End If

            purchaseAmount = Math.Abs(purchaseAmount)

            _log.Debug("Getting transaction id " & logCreditCardTransactionId)

            Dim transactionId As Long
            Dim transaction As CreditCardTransaction
            Dim ccTransaction As TransactionDefinition
            Dim ccDao As ICreditCardDao

            Dim referenceId As String = ""

            If logCreditCardTransactionId > 0 Then

                Try
                    _log.Debug("Reference obtained from logCreditCardTransactionId")

                    ccDao = New CreditCardDao

                    _log.Info("Getting credit card transaction")

                    ccDao.Session.Clear()

                    _log.Debug("Clear session done")

                    transaction = ccDao.GetById(logCreditCardTransactionId)

                    If Not String.IsNullOrWhiteSpace(transaction.ProcessorType) AndAlso Not String.IsNullOrWhiteSpace(_ccProvider) AndAlso transaction.ProcessorType.ToUpper() <> _ccProvider.ToUpper() Then
                        _log.Error("Transaction don't belongs to current provider")
                        Return New ReturnCreditCardResponseDto(ReturnCreditCardResponseDto.ResultCodes.TransactionNotFound)
                    End If

                    referenceId = logCreditCardTransactionId
                Catch ex As Exception
                    _log.Error("Error at get transaction. Message: " + ex.Message, ex)
                    Throw
                End Try
            Else
                _log.Debug("Reference obtained from invoiceId (" & invoiceId & ")")
                referenceId = invoiceId
            End If

            If _bgWorkerIsRunning Then
                _log.Error("Can't process return, authorize transaction is running")
                Return New ReturnCreditCardResponseDto(ReturnCreditCardResponseDto.ResultCodes.PaymentInProgress)
            End If

            Try
                _log.Debug("Transaction Ok, creating authorization transaction")

                ccTransaction = New TransactionDefinition()

                ccTransaction.TransactionType = enuTransactionType.ttCreditReturn
                ccTransaction.amount = purchaseAmount
                ccTransaction.transactionId = referenceId
                ccTransaction.transactionReference = referenceId

                transactionId = _ccEngine.CreateTransaction(ccTransaction, _ccProvider)

                If transactionId <= 0 Then
                    _log.Error("Can't create credit card transaction")
                    Return New ReturnCreditCardResponseDto(ReturnCreditCardResponseDto.ResultCodes.UndefinedError)
                End If

                _log.Debug("SqlTransactionId created: " & transactionId)

                ccTransaction.sqlTransactionId = transactionId
            Catch ex As Exception
                _log.Error("Error at create credit card transaction. Message: " + ex.Message, ex)
                Throw
            End Try

            Try
                _log.Debug("Creating background worker for process payment")
                _bgWorker = New BackgroundWorker()
                _bgWorker.WorkerSupportsCancellation = True
                _bgWorker.RunWorkerAsync(ccTransaction)

                _log.Debug("Background worker created, starting timer for " & TimerTimeout & " miliseconds")

                _timerBgWorker = New Timer(TimerTimeout)
                _timerBgWorker.Start()

                _log.Debug("Timer ok")
            Catch ex As Exception
                _log.Error("Error at create payment thread. Message: " + ex.Message, ex)
                Throw New ApplicationException("Error at create payment process")
            End Try

            Return New ReturnCreditCardResponseDto(ReturnCreditCardResponseDto.ResultCodes.ReturnInProgress, transactionId)
        End Function

        Private sub SendTransaction(transaction As TransactionDefinition)
            _log.Debug("Sending transaction to ccard")

            Dim res = _ccengine.Commit(transaction)

            _log.Debug("Transaction complete with result: " + res.ToString())
        End sub

        Private sub bgWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles _bgWorker.DoWork
            SendTransaction(CType(e.Argument, TransactionDefinition))
        End sub

        Private sub bgWorker_Finished(sender As Object, e As RunWorkerCompletedEventArgs) Handles _bgWorker.RunWorkerCompleted
            _log.Debug("Worker finished")
            _bgWorkerIsRunning = false
        End sub

        Private sub TimerElapsed(sender As Object, e As ElapsedEventArgs) Handles _timerBgWorker.Elapsed
            _log.Debug("Timeout (" & TimerTimeout & ") elapsed, destroying worker and timer")

            try
                _bgWorker.CancelAsync()
                _bgWorker.Dispose()
                _bgWorkerIsRunning = false

                _timerBgWorker.Stop()
                _timerBgWorker.Dispose()
            Catch ex As Exception
                _log.Debug("Can't destroy objects")
            End Try

            _log.Debug("Destroyed objects")
        End sub
    End Class
End Namespace