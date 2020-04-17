Imports System.IO
Imports System.Text

Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCS.mPosEngine.Core.Domain
Imports DCS.mPosEngine.Data.Dao
Imports DCS.ProjectBase.Data.NHibernateSessionMgmt

Namespace Infrastructure
	Public Class InvoiceNumberGenerator
		Private _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

		Private _workMode As WorkModesEnum
		Private _nw As String
		Private _fiscalInterface As FiscalInterface
		Private _invoicePrefix As String
		Private _sequenceNumber As String
		Private _posName As String
		Private _fiscalInterfacePerDevice As Boolean
		Private _fiscalPrefix As String

		Public Enum WorkModesEnum
			GlobalByFlatFile = 1
		End Enum

		Public Enum FiscalInterface
			None = 0
			Colombia = 1
		End Enum

		Public Sub New(Optional posName As String = "")
			_nw = Environ("NW")
			If _nw.Length > 0 And Right(_nw, 1) <> "\" Then
				_nw = _nw & "\"
			End If

			_posName = posName

			_fiscalInterface = FiscalInterface.None
			_fiscalInterfacePerDevice = False
			_fiscalPrefix = String.Empty

			Dim d As New NHibernateDaoFactory
			Dim dao As ISystemConfigDao = d.GetSystemConfigDao()

			Dim systemConfigSettings = dao.FindAll()

			If systemConfigSettings.Any(Function(x) x.Program.ToLower() = "mpos" And x.Section.ToLower() = "general" And x.Key.ToLower() = "fiscalinterface") Then
				Dim fiscalInterface = systemConfigSettings.First(Function(x) x.Program.ToLower() = "mpos" And x.Section.ToLower() = "general" And x.Key.ToLower() = "fiscalinterface").Value

				If fiscalInterface Is Nothing Then
					_log.Warn("Fiscal interface not defined")
					Exit Sub
				End If

				Select Case fiscalInterface.ToLower()
					Case "colombia"
						_fiscalInterface = InvoiceNumberGenerator.FiscalInterface.Colombia

						_log.Info("Fiscal Interface Colombia")
					Case Else
						_log.Warn("Fiscal interface (" + fiscalInterface + ") not defined. Default (none) is setted.")
				End Select
			End If

			If systemConfigSettings.Any(Function(x) x.Program.ToLower() = "mpos" And x.Section.ToLower() = "general" And x.Key.ToLower() = "fiscalinterfaceperdevice") Then
				_fiscalInterfacePerDevice = (systemConfigSettings.First(Function(x) x.Program.ToLower() = "mpos" And x.Section.ToLower() = "general" And x.Key.ToLower() = "fiscalinterfaceperdevice").Value = "1")

				If _fiscalInterfacePerDevice And String.IsNullOrWhiteSpace(_posName) Then
					_log.Error("Fiscal Interface Per Device is activated and PosName is empty.")
					Throw New ApplicationException("Fiscal Interface Per Device is activated and PosName is empty.")
				End If
			End If

			Dim d2 As New NHibernateDaoFactory
			Dim dao2 As IConfigDao = d2.GetConfigDao()

			Dim settings = If(_fiscalInterfacePerDevice, dao2.GetConfigForPos(_posName), dao2.GetConfigForPos(""))

			If settings.Any(Function(x) x.Section.ToLower() = "printer" And x.Key.ToLower() = "prefix") Then
				_fiscalPrefix = settings.First(Function(x) x.Section.ToLower() = "printer" And x.Key.ToLower() = "prefix").SettingValue
			End If
		End Sub

		Public Function GetNextAndIncrement() As String
			Dim data As String
			Dim newValue As Integer
			Dim retVal As String
			Dim invoice As Integer

			Dim filename = "InvoiceNumber_" + If(_fiscalInterfacePerDevice, _posName, "Global"),
				extension = ".seq",
				path = "dcs\data\mPosEngine",
				fullpath = _nw + path + "\" + filename + extension

			CheckAndCreateInvoiceFile(path, fullpath)

			Using sr As StreamReader = File.OpenText(fullpath)
				data = sr.ReadLine
				If data <> vbNullString AndAlso Integer.TryParse(data, invoice) Then
					newValue = invoice + 1
					retVal = invoice.ToString
				Else
					retVal = "1"
					newValue = 2
				End If
			End Using

			Using fs As FileStream = File.Create(fullpath)
				fs.Write((New UTF8Encoding).GetBytes(newValue.ToString), 0, newValue.ToString.Length)
			End Using

			If _fiscalInterface = FiscalInterface.Colombia Then
				retVal = _fiscalPrefix + retVal
			End If

			Return retVal
		End Function

		Private Sub CheckAndCreateInvoiceFile(path As String, fullpath As String)
			Dim utf As New UTF8Encoding

			If Not Directory.Exists(_nw + path) Then
				Directory.CreateDirectory(_nw + path)
			End If

			If Not File.Exists(fullpath) Then
				Using fs As FileStream = File.Create(fullpath)
					fs.Write(utf.GetBytes("1"), 0, 1)
				End Using
			End If
		End Sub
	End Class
End Namespace