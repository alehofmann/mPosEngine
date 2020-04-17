Imports System
Imports System.Drawing
Imports System.IO

Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.mPosEngine.Data.Dao
Imports DCS.mPosEngine.Data
Imports DCS.PlaycardBase.Data
Imports DCS.mPosEngine.Services.Dto
Imports DCS.mPosEngine.Core.Domain
Imports DCS.mPosEngine.Core.Domain.Sales
Imports DCS.ProjectBase.Data.NHibernateSessionMgmt
Imports DCSSecurity
Imports DCS.mPosEngine.Data.Infrastructure
Imports log4net.Filter

Public Class PosServices

	Private _securityEngine As DCSSecurity.AuthorizerEngine
	'Private _posServices As PosServices
	Private _treasuryEngine As ITreasuryEngine
	Private _treasuryEnabled As Boolean
	Private Shared _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	Public Function GetPaymodes() As IList(Of PaymodeDto)
		Dim currencyDao As DCS.PlaycardBase.Core.DataInterfaces.ICurrencyDao = (New DCS.PlaycardBase.Data.NHibernateDaoFactory).GetCurrencyDao

		_log.Info("Processing GetPaymodes")
		_log.Debug("Retrieving paymodes from DB")
		Return Mapper.GetPaymodesDto(currencyDao.FindAll)
	End Function

	Public Function GetPosConfig(ByVal posName As String) As IList(Of PosConfigDto)
		Dim posConfigDao As ConfigDao
		Dim settings As IList(Of PosSetting)


		_log.Info("Processing GetPaymodes")

		_log.Debug("Retrieving from DB config for POS [" & posName & "]")
		posConfigDao = (New Dao.NHibernateDaoFactory).GetConfigDao

		Using unitOfWork As IUnitOfWork = DCS.ProjectBase.Data.NHibernateSessionMgmt.UnitOfWork.Start
			settings = posConfigDao.GetConfigForPos(posName)
			unitOfWork.Commit()
		End Using

		_log.Debug("Building response DTO")
		Return Mapper.GetPosConfigDto(settings)


	End Function

	Public Function RegisterDevice(ByVal deviceSerial As String, ByVal pairCode As String) As RegisterDeviceResponseDto
		Dim deviceDao As DeviceDao
		Dim retVal As New RegisterDeviceResponseDto
		Dim deviceData As PosDevice

		_log.Info("Processing GetPaymodes")

		Using unitOfWork As IUnitOfWork = DCS.ProjectBase.Data.NHibernateSessionMgmt.UnitOfWork.Start
			_log.Debug("Getting from DB device data for device serial [" & deviceSerial & "]")
			deviceDao = (New Dao.NHibernateDaoFactory).GetDeviceDao
			deviceData = deviceDao.GetDeviceBySerial(deviceSerial)

			If deviceData IsNot Nothing Then
				_log.Warn("Device found, serial already registered. Registration failed")
				retVal.ResultCode = RegisterDeviceResponseDto.ResultCodesEnum.SerialAlreadyRegistered
			Else
				_log.Debug("Device not found, now retrieving by pair code")
				deviceData = deviceDao.GetDeviceByPairCode(pairCode)
				If deviceData Is Nothing Then
					_log.Warn("Device not found, pair code is invalid. Registration failed.")
					retVal.ResultCode = RegisterDeviceResponseDto.ResultCodesEnum.InvalidPairCode
				Else
					_log.Warn("Device found, updating DB. Registration success")
					retVal.ResultCode = RegisterDeviceResponseDto.ResultCodesEnum.RegisterOk
					deviceData.SerialNumber = deviceSerial
					deviceData.PairCode = Nothing
					deviceData.Enabled = True
					retVal.DeviceName = deviceData.Name
					retVal.DeviceId = deviceData.Id

					deviceDao.MakePersistent(deviceData)

					unitOfWork.Commit()
				End If
			End If
		End Using

		Return retVal
	End Function
	Public Function CheckDevice(ByVal deviceSerial As String) As CheckDeviceResponseDto
		Dim deviceDao As DeviceDao
		Dim retVal As New CheckDeviceResponseDto
		Dim deviceData As PosDevice

		_log.Info("Processing CheckDevice (deviceSerial=[" & deviceSerial & "])")
		'deviceDao.Session.Refresh(deviceData)

		Using unitOfWork As IUnitOfWork = DCS.ProjectBase.Data.NHibernateSessionMgmt.UnitOfWork.Start
			_log.Debug("Getting device from DB")
			deviceDao = (New Dao.NHibernateDaoFactory).GetDeviceDao
			deviceData = deviceDao.GetDeviceBySerial(deviceSerial)
			unitOfWork.Commit()
		End Using

		If deviceData Is Nothing Then
			_log.Warn("Device with serial [" & deviceSerial & "] not found im DB, invalid serial number")
			retVal.ResultCode = CheckDeviceResponseDto.ResultCodesEnum.InvalidSerialNumber
		Else
			retVal.DeviceName = deviceData.Name
			retVal.DeviceId = deviceData.Id
			retVal.ComputerId = deviceData.ComputerId

			If Not deviceData.Enabled Then
				_log.Info("Device found and is disabled")
				retVal.ResultCode = CheckDeviceResponseDto.ResultCodesEnum.DeviceIsDisabled
			Else
				_log.Info("Device found and is enabled")
				retVal.ResultCode = CheckDeviceResponseDto.ResultCodesEnum.DeviceValidAndEnabled
			End If
		End If



		Return retVal
	End Function

	'Public Function GetAdminFunctions2(ByVal userCardNumber As Integer) As GetAdminFunctionsResponseDto
	'    Dim retVal As New GetAdminFunctionsResponseDto

	'    retVal.CommandSuccess = True
	'    retVal.ErrorDescription = "nada"
	'    retVal.Functions = GetAdminFunctions(userCardNumber)

	'    Return retVal
	'End Function
	Public Function GetAdminFunctions(ByVal userCardNumber As Integer) As IList(Of AdminFunctionDto)

		Dim adminDao As AdminFunctionDao
		Dim adminFunction As AdminFunction
		Dim retVal As IList(Of AdminFunctionDto) = New List(Of AdminFunctionDto)
		Dim adminFunctionDto As AdminFunctionDto
		Dim authenticationRequired As Boolean
		Dim adminFunctions As IList(Of AdminFunction)
		Dim user As User = Nothing

		_log.Info("Processing GetAdminFunctions")

		Using unitOfWork As IUnitOfWork = DCS.ProjectBase.Data.NHibernateSessionMgmt.UnitOfWork.Start
			_log.Debug("Retrieving admin functions from DB")
			adminDao = (New Dao.NHibernateDaoFactory).GetAdminFunctionDao
			adminFunctions = adminDao.FindAll
			unitOfWork.Commit()
		End Using

		For Each adminFunction In adminFunctions
			_log.Debug("Mapping to response DTO")
			If adminFunction.ForceReenterCredentials Then
				authenticationRequired = True
			Else
				authenticationRequired = (CheckAccess(adminFunction.SecurityActionId, userCardNumber, user).ResultCode = CheckAccessResponseDto.ResultCodesEnum.AccessDenied)
			End If

			'Map Internal To DTO***********
			adminFunctionDto = New AdminFunctionDto
			adminFunctionDto.FunctionId = adminFunction.Id
			adminFunctionDto.FunctionName = adminFunction.Name
			adminFunctionDto.AuthenticationRequired = authenticationRequired
			adminFunctionDto.SecurityActionId = adminFunction.SecurityActionId
			adminFunctionDto.ThumbUrl = adminFunction.ThumbUrl
			'******************************            

			retVal.Add(adminFunctionDto)
		Next

		Return retVal
	End Function

	Public Sub New()
		'_posServices = New PosServices

		_log.Info("Instancing security engine")
		Try
			_securityEngine = New DCSSecurity.AuthorizerEngine
		Catch ex As Exception
			Throw New ApplicationException("Error Instancing Security engine", ex)
		End Try

		_log.Info("Creating Security Engine (NW=[" & Environ("NW") & "])")

		Try
			If Not _securityEngine.Create(Environ("NW")) Then
				Throw New ApplicationException("Security engine returned an error: " & _securityEngine.LastErrorString & " in line " & _securityEngine.LastErrorNumber)
			End If

		Catch ex As Exception
			Throw New ApplicationException("Error Creating Security engine", ex)
		End Try

		_log.Info("Creating Config Engine")
		Dim configEngine As New ConfigEngine.Engine("POSEngine")

		_treasuryEnabled = CBool(configEngine.GetItem("Treasury", "Enabled", 0))
		If _treasuryEnabled Then
			_log.Info("Treasury is enabled, instancing Treasury Engine")
			Try
				_treasuryEngine = New DCS.mPosEngine.Data.Infrastructure.TreasuryEngine
			Catch ex As Exception
				Throw New ApplicationException("Error Creating Treasury engine", ex)
			End Try
		Else
			_log.Warn("Treasury is disabled")
		End If
	End Sub

	Public Function LoginCashierOnlyForTests(ByVal loginMode As String, ByVal cardNumber As Integer) As LoginDataDto
		Dim treasuryWasDisabled As Boolean

		Try
			If Not _treasuryEnabled Then
				treasuryWasDisabled = True
				_treasuryEnabled = True
				_treasuryEngine = New DCS.mPosEngine.Data.Infrastructure.TreasuryEngine
			Else
				treasuryWasDisabled = False
			End If

			Return LoginCashier(loginMode, cardNumber)

		Finally
			If treasuryWasDisabled Then
				_treasuryEnabled = False
				_treasuryEngine = Nothing
			End If

		End Try

	End Function
	Public Function LoginCashier(ByVal loginMode As String, ByVal cardNumber As Integer) As LoginDataDto
		Dim retVal As New LoginDataDto
		Dim user As User
		Dim result As Boolean

		_log.Info("Processing LoginCashier (cardNumber=" & cardNumber & ")")

		If cardNumber > 0 Then
			_log.Debug("Checking if user is allowed to login")
			result = IsUserAllowed(1, cardNumber, user)

			If result Then
				retVal.CashierCardNumber = cardNumber
				retVal.CashierFirstName = user.FirstName
				retVal.CashierId = user.Id
				retVal.CashierLastName = user.LastName
				retVal.CashierLoginName = user.Handle

				If _treasuryEnabled Then
					_log.Info("Treasury is enabled, processing login in treasury")
					Select Case _treasuryEngine.LoginCashier(user.Id)
						Case ITreasuryEngine.LoginResultCodesEnum.LoginSuccess
							_log.Info("Login success")
							retVal.ResultCode = LoginDataDto.ResultCodesEnum.LoginSuccess
						Case ITreasuryEngine.LoginResultCodesEnum.NoOpenSession
							_log.Warn("Login denied, no open treasury session")
							retVal.ResultCode = LoginDataDto.ResultCodesEnum.LoginDeniedNoOpenTreasurySession
						Case ITreasuryEngine.LoginResultCodesEnum.InvalidCashierId
							_log.Warn("Login denied, invalid treasury cashier id")
							retVal.ResultCode = LoginDataDto.ResultCodesEnum.LoginInvalidTreasuryCashierId
					End Select
				Else
					_log.Warn("Login success")
					retVal.ResultCode = LoginDataDto.ResultCodesEnum.LoginSuccess
				End If
			Else
				_log.Warn("Login denied, invalid credentials or user not authorized to login")
				retVal.ResultCode = LoginDataDto.ResultCodesEnum.LoginDeniedInvalidCredentials
			End If
		Else
			_log.Error("cardNumber must be > 0. Throwing exception")
			Throw New ApplicationException("cardNumber must be > 0")
		End If

		Return retVal
	End Function

	Public Function CheckAccess(ByVal actionId As Integer, ByVal userCardNumber As Integer, Optional ByRef user As User = Nothing) As CheckAccessResponseDto
		Dim retVal As New CheckAccessResponseDto

		retVal = New CheckAccessResponseDto(IsUserAllowed(actionId, userCardNumber, user))

		Return retVal

	End Function

    Public Function IsUserAllowed(ByVal actionId As Integer, ByVal userCardNumber As Integer, Optional ByRef user As User = Nothing) As Boolean
        Dim retVal As Boolean
        Dim userDao As IUserDao = (New Dao.NHibernateDaoFactory).GetUserDao
        Dim returnedUser As clsUser

        If userCardNumber <= 0 Then
            Throw New ArgumentException("Invalid CardNumber: " & userCardNumber, "userCardNumber")
        End If

        If user Is Nothing Then
            'Using unitOfWork As IUnitOfWork = DCS.ProjectBase.Data.NHibernateSessionMgmt.UnitOfWork.Start
            '	_log.Debug("Retrieving user with card " & userCardNumber & " from DB")
            '	user = userDao.GetByCardNumber(userCardNumber)
            '	unitOfWork.Commit()
            'End Using

            _log.Debug("Retrieving user with card " & userCardNumber & " from DB")
            user = userDao.GetByCardNumber(userCardNumber)

            If user Is Nothing Then
                retVal = False
                _log.Warn("User not found in DB")
            End If
        End If

        If Not user Is Nothing Then
            _log.Debug("Invoking Security Engine for actionId " & actionId)
            retVal = _securityEngine.IsUserPermitted(7, actionId, user.Handle, returnedUser, "", userCardNumber)
        End If

        Return retVal
    End Function

    Public Function GetInvoiceImage(imageName As String) As InvoiceImageResponseDto
        Dim res As InvoiceImageResponseDto

        Dim env = Environment.GetEnvironmentVariable("NW")

        If String.IsNullOrWhiteSpace(env) Then
            _log.Error("NW not setted")
            Return New InvoiceImageResponseDto(InvoiceImageResponseDto.ResultCode.PathNotFound)
        End If

        Dim path = env + "\dcs\data\mposengine\"

        If Not Directory.Exists(path) Then
            _log.Error("Path (" & path & ") not found")
            Return New InvoiceImageResponseDto(InvoiceImageResponseDto.ResultCode.PathNotFound)
        End If

        Dim pathFile = path + imageName

        If Not File.Exists(pathFile) Then
            _log.Error("File (" & pathFile & ") not found")
            Return New InvoiceImageResponseDto(InvoiceImageResponseDto.ResultCode.FileNotFound)
        End If

        Try
            Dim im = Image.FromFile(pathFile)
            Using ms As New MemoryStream()
                im.Save(ms, Imaging.ImageFormat.Jpeg)
                Dim base64 = Convert.ToBase64String(ms.ToArray())
                res = New InvoiceImageResponseDto(InvoiceImageResponseDto.ResultCode.Ok, base64)
                im.Dispose()
            End Using
        Catch ex As Exception
            _log.Error("Error at convert image. Message: " + ex.Message, ex)
            res = New InvoiceImageResponseDto(InvoiceImageResponseDto.ResultCode.UnknownError)
        End Try

        Return res
    End Function
End Class