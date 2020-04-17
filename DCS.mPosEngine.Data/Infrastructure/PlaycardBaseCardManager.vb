Imports System.IO
Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.mPosEngine.Core.Domain.Sales.Fulfillment
Imports DCS.PlaycardBase.CardData
Imports DCS.Library

Namespace Infrastructure
	Public Class PlaycardBaseCardManager
		Implements ICardManager

		Private _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

		Private _cardEngine As DCS.CardEngine.Engine
		Private _cardFactory As CardFactory

		Private ReadOnly _localPrefix As String
        Private ReadOnly _maxCardNumber As Long

        Public Sub New()
			_log.Info("Creating CardEngine")
			_cardEngine = New DCS.CardEngine.Engine("mPos")
			_log.Info("Creating CardFactory")
			_cardFactory = New CardFactory

			Dim scardIniPath = Environment.GetEnvironmentVariable("NW") + "\data\scard.ini"

			If Not File.Exists(scardIniPath) Then
				_log.Error("Scard.ini not exists")
				Throw New ApplicationException("Scard.ini not exists")
			End If

			_localPrefix = Ini.GetValue("International", "PrefixLetter", "", scardIniPath) + Ini.GetValue("International", "PrefixNumber", "", scardIniPath)

			If String.IsNullOrWhiteSpace(_localPrefix) Then
				_log.Error("Local Prefix is empty")
				Throw New ApplicationException("Local Prefix is empty")
			End If

			_maxCardNumber = GetValidDatMaxNumber()

			If _maxCardNumber < 1 Then
				_log.Error("Max card number is < 0 (value: " & _maxCardNumber & ")")
				Throw New ApplicationException("Max card number is < 0 (value: " & _maxCardNumber & ")")
			End If
		End Sub

		Public Sub ChangeCardStatus(cardNumber As Long, newStatusId As Integer) Implements ICardManager.ChangeCardStatus
			'Dim card As Card = _cardFactory.GetCard(CInt(cardNumber))
			Dim card = GetCard(cardNumber)

			If card Is Nothing Then
				Throw New InvalidOperationException("Card is nothing. Cannot change card status for CardNumber: " & cardNumber)
			End If

			Dim daoFactory As New PlaycardBase.Data.NHibernateDaoFactory
			Dim playcardStatusDao = daoFactory.GetPlaycardStatusDao
			Dim newStatus = playcardStatusDao.GetById(newStatusId)

			If newStatus Is Nothing Then
				Throw New InvalidOperationException("Could not find status for StatusID " & newStatusId)
			End If

			card.SetStatus(newStatus.LegacyId)
			card.Update()
		End Sub

		Private Function GetCounterType(ByVal counterTypeId As Integer) As DCS.PlaycardBase.Core.GeneralDomain.CounterType
			Dim counterTypeDao As DCS.PlaycardBase.Core.DataInterfaces.ICounterTypeDao = New DCS.PlaycardBase.Data.CounterTypeDao

			Return counterTypeDao.FindById(counterTypeId)
		End Function

		Public Sub ChargeCard(cardNumber As Long, counterTypeId As Integer, amountToCharge As Decimal) Implements ICardManager.ChargeCard
			'Dim card As Card = _cardFactory.GetCard(CInt(cardNumber))
			Dim card = GetCard(cardNumber)

			card.ChargeCard(GetCounterType(counterTypeId), amountToCharge)
			card.Update()
		End Sub

		Public Function IsCardSold(cardNumber As Long) As Boolean Implements ICardManager.IsCardSold
			'Dim card As Card = _cardFactory.GetCard(CInt(cardNumber))
			Dim card = GetCard(cardNumber)

			Return card.ValidInThisStore
		End Function

		Public Sub SellCard(cardNumber As Long) Implements ICardManager.SellCard
			'Dim card As Card = _cardFactory.GetCard(CInt(cardNumber))
			Dim card = GetCard(cardNumber)

			card.Sell()
			card.Update()
		End Sub

		Public Sub WipeCard(cardNumber As Long) Implements ICardManager.WipeCard
			'Dim card As Card = _cardFactory.GetCard(CInt(cardNumber))
			Dim card = GetCard(cardNumber)

			card.Reset()
			card.Update()
		End Sub

		Public Function GetCardCounterAmount(cardNumber As Long, counterTypeId As Integer) As Decimal Implements ICardManager.GetCardCounterAmount
			'Dim card As Card = _cardFactory.GetCard(CInt(cardNumber))
			Dim card = GetCard(cardNumber)

			Return card.GetCounterAmount(GetCounterType(counterTypeId))
		End Function

		Public Function GetCardInfo(cardNumber As Long) As Core.Domain.Sales.Fulfillment.CardInfo Implements ICardManager.GetCardInfo
			'Dim card As Card = _cardFactory.GetCard(CInt(cardNumber))
			Dim card = GetCard(cardNumber)

			Return New CardInfo(card.CardNumber, card.ValidInThisStore)
		End Function

		Public Sub DebitFromCard(cardNumber As Long, counterTypeId As Integer, amountToDebit As Decimal) Implements Core.DataInterfaces.ICardManager.DebitFromCard
			'Dim card As Card = _cardFactory.GetCard(CInt(cardNumber))
			Dim card = GetCard(cardNumber)

			card.ChargeCard(GetCounterType(counterTypeId), amountToDebit * -1)
			card.Update()
		End Sub

		Public Function ConsolidateCards(sourceCardNumbers As Collection, destCardNumber As Long, posId As String, operatorId As Integer, Optional ByRef failedCard As Long = 0) As ICardManager.CardTransferResultCodesEnum Implements Core.DataInterfaces.ICardManager.ConsolidateCards
			Return TransferOrConsolidate(sourceCardNumbers, destCardNumber, posId, operatorId, True, failedCard)
		End Function

		Private Function TransferOrConsolidate(sourceCardNumbers As Collection, destCardNumber As Long, posId As String, operatorId As Integer, ByVal isConsolidate As Boolean, Optional ByRef failedCard As Long = 0) As ICardManager.CardTransferResultCodesEnum
			Try
				_cardEngine.CardTransfer(sourceCardNumbers, destCardNumber, posId, operatorId, isConsolidate)
			Catch ex As CustomException
				Select Case ex.ErrorCode
					Case 1, 31 'already sold, service card
						Long.TryParse(ex.Param1, failedCard)
						Return ICardManager.CardTransferResultCodesEnum.InvalidSourceCard
					Case 3, 4, 32 'destination is not active, destination HAS to be active, destination is service
						Return ICardManager.CardTransferResultCodesEnum.InvalidDestinationCard
					Case Else
						Throw New ApplicationException("Unknown resultreturned by 'CardTransfer': " & ex.ErrorCode)
				End Select
			End Try

			Return ICardManager.CardTransferResultCodesEnum.TransferSuccess
		End Function

		Public Function TransferCard(sourceCardNumber As Long, destCardNumber As Long, posId As String, operatorId As Integer) As ICardManager.CardTransferResultCodesEnum Implements Core.DataInterfaces.ICardManager.TransferCard
			Dim sourceCardCollection As New Collection

			sourceCardCollection.Add(sourceCardNumber)

			Return TransferOrConsolidate(sourceCardCollection, destCardNumber, posId, operatorId, False)
		End Function

		Public Function GetCard(cardNumber As Long) As Card
			If cardNumber > _maxCardNumber Then
				_log.Error("CardNumber " + cardNumber.ToString() + " is not permitted for valid.dat")
				Throw New ApplicationException("CardNumber " + cardNumber.ToString() + " is not permitted for valid.dat")
			End If

			Return _cardFactory.GetCard(CInt(cardNumber))
		End Function

		Private Function GetValidDatMaxNumber() As Long
			Dim path = Environment.GetEnvironmentVariable("NW") + "\data\valid.dat", content As String

			If Not File.Exists(path) Then
				Throw New ApplicationException("Valid.dat not exists")
			End If

			Using sr As New StreamReader(path)
				content = sr.ReadToEnd()
			End Using

			content = content.Trim().Replace(Environment.NewLine, String.Empty)

            _log.Debug("Prefix: " + _localPrefix)
            _log.Debug("Valid.Dat: " + content)

            Dim maxCardNumber = GetMaxCardNumber(content, _localPrefix)

            _log.Debug("MaxCardNumber: " & maxCardNumber)

            Return maxCardNumber
        End Function

        Private Function GetMaxCardNumber(sValidDatLine As String, sCardPrefix As String) As Long
            Dim lRetVal As Long,
                sLeftPart As String,
                lMaxCard As Long,
                sRightPart As String,
                sValid1 As String,
                sValid2 As String,
                sValid3 As String,
                sCorrectValid1 As String,
                sCorrectValid2 As String,
                sCorrectValid3 As String,
                iI As Integer

            'lRetVal will give either the max card usable or a negative number.
            'This negative number is the step where checks did not match.

            'See if sValidDatLine has a valid len
            sValidDatLine = UCase$(sValidDatLine)
            If Len(sValidDatLine) = 0 Then
                lRetVal = -1
                GoTo Xit
            End If

            'See if there is an equals sign
            If InStr(sValidDatLine, "=") = 0 Then
                lRetVal = -2
                GoTo Xit
            End If

            'Extract tentative Card Number (before equals sign)
            sLeftPart = Trim(Left(sValidDatLine, InStr(sValidDatLine, "=") - 1))
            'See if range checks
            lMaxCard = Val(sLeftPart)
            If lMaxCard < 200000 Or lMaxCard > 99999999 Then
                lRetVal = -3
                GoTo Xit
            End If

            'Extract Validator (after equals sign)
            sRightPart = Trim(Mid(sValidDatLine, InStr(sValidDatLine, "=") + 1))
            If Len(sRightPart) <> 12 Then
                lRetVal = -4
                GoTo Xit
            End If

            'Ready input data
            sCardPrefix = UCase(sCardPrefix)
            If Len(sCardPrefix) <> 2 Then
                lRetVal = -5
                GoTo Xit
            End If

            'If Not IsNumeric(Right(sCardPrefix, 1)) Then
            '	lRetVal = -5
            '	GoTo Xit
            'End If

            'If Left(sCardPrefix, 1) < "A" Or Left(sCardPrefix, 1) > "Z" Then
            '	lRetVal = -5
            '	GoTo Xit
            'End If

            'Unjumble all three validators V1, V2 and V3 from Valid.Dat
            sValid1 = vbNullString
            sValid2 = vbNullString
            sValid3 = vbNullString
            For iI = Len(sRightPart) To 1 Step -3
                sValid1 = sValid1 & Mid(sRightPart, iI, 1)
                sValid2 = sValid2 & Mid(sRightPart, iI - 1, 1)
                sValid3 = sValid3 & Mid(sRightPart, iI - 2, 1)
            Next iI

            'Calculate the right validators based upon card number & prefix
            sCorrectValid1 = GenV1(lMaxCard, sCardPrefix)
            sCorrectValid2 = GenV2(lMaxCard + Val("&H" & sCorrectValid1 & "&"))
            sCorrectValid3 = GenV3(lMaxCard + Val("&H" & sCorrectValid2 & "&"))

            'Fail with different exit codes if any validator segment does not match
            If sValid1 <> sCorrectValid1 Then
                lRetVal = -10
                GoTo Xit
            End If
            If sValid2 <> sCorrectValid2 Then
                lRetVal = -20
                GoTo Xit
            End If
            If sValid3 <> sCorrectValid3 Then
                lRetVal = -30
                GoTo Xit
            End If

            'If we did get here, then we're home free.
            lRetVal = lMaxCard

Xit:
            Return lRetVal
        End Function

        Private Function GenV1(lCardNo As Long, sPrefix As String) As String
			Dim sRetVal As String,
				byByte As Byte,
				iI As Integer,
				iJ As Integer,
				iK As Integer,
				aBit(16) As Byte,
				sStream As String,
				byBit As Byte,
				lX As Long

			sRetVal = vbNullString

			For iI = 1 To 16
				aBit(iI) = 0
			Next iI
			aBit(16) = 1

			sStream = "soXa" & CStr(lCardNo) & sPrefix & "1hs!"

			For iI = Len(sStream) To 1 Step -1
				'Fetch next byte to work on
				byByte = Asc(Mid(sStream, iI, 1))

				'Cycle all bits in the byte
				For iJ = 0 To 7
					'Isolate bit
					byBit = byByte And 2 ^ iJ

					'Flip selected bits in result word depending on current bit set or reset
					'If byBit = 0 Then
					'  aBit(0) = 0 Xor (aBit(16) Xor aBit(12) Xor aBit(9) Xor aBit(7))
					'Else
					'  aBit(0) = 1 Xor (aBit(16) Xor aBit(12) Xor aBit(9) Xor aBit(7))
					'End If
					'ML's replacement to above IF
					aBit(0) = Math.Sign(byBit) Xor (aBit(16) Xor aBit(12) Xor aBit(9) Xor aBit(7))

					'Shift result word left (brings a 0 to aBit(1) from aBit(0) which is never set or reset); original aBit(16) is lost
					'Please note that a simple OPTION BASE statement may cause this part to abort (invalid index when iK-1=0).
					For iK = 16 To 1 Step -1
						aBit(iK) = aBit(iK - 1)
					Next iK
				Next iJ
			Next iI

			'Convert binary to decimal, then hexa, then return it
			lX = 0
			For iI = 1 To 16
				lX = lX + aBit(iI) * (2 ^ (16 - iI))
			Next iI
			sRetVal = Right("0000" & Hex(lX), 4)

			Return sRetVal
		End Function

		Private Function GenV2(lNumber As Long) As String
			Dim sRetVal As String,
				byByte As Byte,
				iI As Integer,
				iJ As Integer,
				iK As Integer,
				aBit(16) As Byte,
				sStream As String,
				byBit As Byte,
				lX As Long

			sRetVal = vbNullString

			For iI = 1 To 16
				aBit(iI) = 0
			Next iI
			aBit(16) = 1

			sStream = ".soXa" & CStr(lNumber) & "30m!"

			For iI = Len(sStream) To 1 Step -1
				'Fetch next byte to work on
				byByte = Asc(Mid(sStream, iI, 1))

				'Cycle all bits in the byte
				For iJ = 0 To 7
					'Isolate bit
					byBit = byByte And 2 ^ iJ

					'Flip selected bits in result word depending on current bit set or reset
					'If byBit = 0 Then
					'  aBit(0) = 0 Xor (aBit(16) Xor aBit(12) Xor aBit(9) Xor aBit(7))
					'Else
					'  aBit(0) = 1 Xor (aBit(16) Xor aBit(12) Xor aBit(9) Xor aBit(7))
					'End If
					'ML's replacement to above IF
					aBit(0) = Math.Sign(byBit) Xor (aBit(16) Xor aBit(12) Xor aBit(9) Xor aBit(7))

					'Shift result word left (brings a 0 to aBit(1) from aBit(0) which is never set or reset); original aBit(16) is lost
					'Please note that a simple OPTION BASE statement may cause this part to abort (invalid index when iK-1=0).
					For iK = 16 To 1 Step -1
						aBit(iK) = aBit(iK - 1)
					Next iK
				Next iJ
			Next iI

			'Convert binary to decimal, then hexa, then return it
			lX = 0
			For iI = 1 To 16
				lX = lX + aBit(iI) * (2 ^ (16 - iI))
			Next iI
			sRetVal = Right("0000" & Hex(lX), 4)

			Return sRetVal
		End Function

		Private Function GenV3(lNumber As Long) As String
			Dim sRetVal As String,
				byByte As Byte,
				iI As Integer,
				iJ As Integer,
				iK As Integer,
				aBit(16) As Byte,
				sStream As String,
				byBit As Byte,
				lX As Long

			'Dim vByte As Integer, i As Integer, j As Integer, k As Integer
			'Dim b(16), DataStream$, bit As Integer, x&

			sRetVal = vbNullString

			For iI = 1 To 16
				aBit(iI) = 0
			Next iI
			aBit(16) = 1

			sStream = "..oXa " & CStr(lNumber) & "Cancel"

			For iI = Len(sStream) To 1 Step -1
				'Fetch next byte to work on
				byByte = Asc(Mid(sStream, iI, 1))

				'Cycle all bits in the byte
				For iJ = 0 To 7
					'Isolate bit
					byBit = byByte And 2 ^ iJ

					'Flip selected bits in result word depending on current bit set or reset
					'If byBit = 0 Then
					'  aBit(0) = 0 Xor (aBit(16) Xor aBit(12) Xor aBit(9) Xor aBit(7))
					'Else
					'  aBit(0) = 1 Xor (aBit(16) Xor aBit(12) Xor aBit(9) Xor aBit(7))
					'End If
					'ML's replacement to above IF
					aBit(0) = Math.Sign(byBit) Xor (aBit(16) Xor aBit(12) Xor aBit(9) Xor aBit(7))

					'Shift result word left (brings a 0 to aBit(1) from aBit(0) which is never set or reset); original aBit(16) is lost
					'Please note that a simple OPTION BASE statement may cause this part to abort (invalid index when iK-1=0).
					For iK = 16 To 1 Step -1
						aBit(iK) = aBit(iK - 1)
					Next iK
				Next iJ
			Next iI

			'Convert binary to decimal, then hexa, then return it
			lX = 0
			For iI = 1 To 16
				lX = lX + aBit(iI) * (2 ^ (16 - iI))
			Next iI
			sRetVal = Right("0000" & Hex(lX), 4)

			Return sRetVal
		End Function
	End Class
End Namespace