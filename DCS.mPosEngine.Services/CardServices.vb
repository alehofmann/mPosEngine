Imports System

Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.mPosEngine.Services.Dto
Imports DCS.mPosEngine.Data.Infrastructure
Imports DCS.PlaycardBase.CardData
Imports DCS.PlaycardBase.Data

Public Class CardServices
	Private _cf As CardFactory
	Private _cm As ICardManager
	Private _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
	'Private _treasuryEngine As ITreasuryEngine
	'Private _treasuryEnabled As Boolean

	Public Function CardConsolidate(ByVal command As CardConsolidateCommandDto) As CardTransferResponseDto
		Dim result As ICardManager.CardTransferResultCodesEnum
		Dim sourceCards As New Collection
		Dim failedCard As Long

		_log.Info("Processing CardConsolidate")

		_log.Debug("Building source cards collection")
		For Each cardNumber As Long In command.SourceCards
			sourceCards.Add(cardNumber)
		Next
		_log.Debug("Collection created: " & sourceCards.Count & " source cards")

		_log.Info("Calling CardManager, consolidating cards")
		result = _cm.ConsolidateCards(sourceCards, command.DestCard, command.MPosName, command.OperatorId, failedCard)
		Select Case result

			Case ICardManager.CardTransferResultCodesEnum.InvalidSourceCard
				_log.Warn("Card Manager returned: InvalidSourceCard")
				Return New CardTransferResponseDto(CardTransferResponseDto.ResultCodesEnum.InvalidSourceCard, failedCard)
			Case ICardManager.CardTransferResultCodesEnum.InvalidDestinationCard
				_log.Warn("Card Manager returned: InvalidDestinationCard")
				Return New CardTransferResponseDto(CardTransferResponseDto.ResultCodesEnum.InvalidDestinationCard)
			Case ICardManager.CardTransferResultCodesEnum.TransferSuccess
				_log.Info("Card Manager returned: TransferSuccess")
				Return New CardTransferResponseDto(CardTransferResponseDto.ResultCodesEnum.TransferSuccess)
			Case Else
				_log.Error("Card Manager returned an error: [" & result & "]")
				Throw New ApplicationException("Unknown result code returned by CardManager: " & result)
		End Select
	End Function
	Public Function CardTransfer(ByVal command As CardTransferCommandDto) As CardTransferResponseDto
		Dim result As ICardManager.CardTransferResultCodesEnum
		_log.Info("Processing Card Transfer (SourceCard=" & command.SourceCard & ",  DestinationCard=" & command.DestCard & ")")

		_log.Info("Calling CardManager, transferring card " & command.SourceCard & " to card " & command.DestCard)
		result = _cm.TransferCard(command.SourceCard, command.DestCard, command.MPosName, command.OperatorId)
		Select Case result

			Case ICardManager.CardTransferResultCodesEnum.InvalidSourceCard
				_log.Warn("Card Manager returned: InvalidSourceCard")
				Return New CardTransferResponseDto(CardTransferResponseDto.ResultCodesEnum.InvalidSourceCard)
			Case ICardManager.CardTransferResultCodesEnum.InvalidDestinationCard
				_log.Warn("Card Manager returned: InvalidDestinationCard")
				Return New CardTransferResponseDto(CardTransferResponseDto.ResultCodesEnum.InvalidDestinationCard)
			Case ICardManager.CardTransferResultCodesEnum.TransferSuccess
				_log.Info("Card Manager returned: TransferSuccess")
				'If _treasuryEnabled Then
				'    _log.Info("Commiting card transfer to treasury")
				'    _treasuryEngine.
				'    'Registrar el transfer
				'End If

				Return New CardTransferResponseDto(CardTransferResponseDto.ResultCodesEnum.TransferSuccess)
			Case Else
				_log.Error("Card Manager returned an error: [" & result & "]")
				Throw New ApplicationException("Unknown result code returned by CardManager: " & result)
		End Select
	End Function

	Public Function GetCardStatus(ByVal cardNuMber As Integer) As CardStatusDto
		Dim retVal As New CardStatusDto
		Dim card As DCS.PlaycardBase.CardData.Card

		_log.Info("Processing GetCardStatus (cardNumber=" & cardNuMber & ")")

		_log.Debug("Getting card from CardFactory")
		card = _cf.GetCard(cardNuMber)

		_log.Debug("Checking Card Status")

		If Not card.ValidInThisStore Then
			_log.Info("Card " & cardNuMber & " is a new card")
			retVal.CardStatus = CardStatusDto.CardStatusesEnum.NewCard
		ElseIf UCase(card.CDTData.Status = "S") Then
			_log.Info("Card " & cardNuMber & " is a service card")
			retVal.CardStatus = CardStatusDto.CardStatusesEnum.ServiceCard
		Else
			_log.Info("Card " & cardNuMber & " is a valid sold card")
			retVal.CardStatus = CardStatusDto.CardStatusesEnum.ValidSoldCard
		End If

		Return retVal
	End Function
	Public Function CardAnalyze(ByVal cardNumber As Integer) As CardDataDto
		Dim retVal As New CardDataDto
		Dim passport As PassportItemDto
		'Dim cm As DCS.mPosEngine.Core.DataInterfaces.ICardManager
		Dim card As DCS.PlaycardBase.CardData.Card
		'Dim cardInfo As cardinfo

		_log.Info("Processing CardAnalyze (cardNumber=" & cardNumber & ")")
		If cardNumber <= 0 Then
			_log.Error("cardNumber must be a positive integer number, aborting CardAnalyze")
			Throw New ArgumentException("cardNumber must be a positive integer number", "cardNumber")
		End If

		_log.Debug("Getting card from CardFactory")
		'Aca tendria que usar el CardManager
		card = _cf.GetCard(cardNumber)
		'cm = CardManagerFactory.GetCardManager

		retVal.CardNumber = cardNumber
		retVal.CardStatus = card.CDTData.Status
		retVal.Credits = card.CDTData.Balance
		retVal.Bonus = card.CDTData.Bonus
		retVal.Courtesy = card.CDTData.Courtesy
		retVal.Tickets = card.CDTData.Tickets
        retVal.Minutes = card.CDTData.RemaniningTime.TotalMinutes
        retVal.PlayAmountToPromote = card.PlayAmountToPromote

        _log.Debug("Getting card passports")
		card.PassportData.GetPassports()

		_log.Debug("Building card passport data")
		For Each passportItem As PassportItem In card.PassportData.GetPassports()
			retVal.Passports.Add(New PassportItemDto(passportItem.PassportName, passportItem.AvailableAmount))
		Next

		Return retVal
	End Function

	Public Function GetCardHistory(ByVal cardNumber As Long, ByVal maxLines As Integer) As IList(Of CardHistoryLineDto)
		_log.Info("Processing GetCardHistory (cardNumber=" & cardNumber & ")")
		'Return Mapper.GetCardHistoryDto((New DCS.PlaycardBase.Data.NHibernateDaoFactory).GetCardHistoryDao.GetByCardNumber(cardNumber, maxLines))

		Dim daoF As New NHibernateDaoFactory

		_log.Debug("New NHibernateDaoFactory")

		Dim cardHistory = daoF.GetCardHistoryDao().GetByCardNumber(cardNumber, maxLines)

		_log.Debug("CardHistory rows: " & cardHistory.Count)

		Return Mapper.GetCardHistoryDto(cardHistory)
	End Function

    Public Sub New()
        _log.Info("Creating Card Manager")
        _cm = CardManagerFactory.GetCardManager

        _log.Info("Creating CardFactory")
        _cf = New CardFactory

		'     _treasuryEnabled = CBool(DCS.ConfigEngine.GetIni("Treasury", "Enabled", 0))
		'     If _treasuryEnabled Then
		'         _log.Info("Treasury is enabled, instancing Treasury Engine")
		'         Try
		'	_treasuryEngine = New TreasuryEngine
		'Catch ex As Exception
		'             Throw New ApplicationException("Error Creating Treasury engine", ex)
		'         End Try
		'     Else
		'         _log.Warn("Treasury is disabled")
		'     End If
	End Sub
End Class
