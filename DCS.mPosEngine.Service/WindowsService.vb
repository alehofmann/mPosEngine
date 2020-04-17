Imports System.ServiceModel.Web
Imports System.ServiceModel.Description
Imports System.ServiceModel
Imports System.Windows.Forms
Imports System.Reflection
Imports System.Threading

Public Class DcsMposEngineWebService
    Inherits System.ServiceProcess.ServiceBase

    Private Shared _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    'Private _startTimer As System.Timers.Timer
    Private _mainThread As Thread

	Protected Overrides Sub OnStart(ByVal args() As String)
		' Add code here to start your service. This method should set things
		' in motion so your service can do its work.

        thread.Sleep(10000)

		_log.Info("ONSTART: Thread Aparment State is: " & Threading.Thread.CurrentThread.GetApartmentState.ToString)

        '_startTimer = New System.Timers.Timer
        '_startTimer.Interval = 1000
        'AddHandler _startTimer.Elapsed, AddressOf Tick
        '_startTimer.Start()

        'Thread.Sleep(15000)

        If Not InitializeProgram() Then
            MyBase.ExitCode = 1064
            MyBase.Stop()
        End If

        '_mainThread = new Thread(AddressOf InitializeProgram)
        '_mainThread.SetApartmentState(ApartmentState.STA)
        '_mainThread.Start()
    End Sub

	'Private Sub Tick(sender As Object, e As ElapsedEventArgs)
	'	Dim thread As Thread = New Thread(AddressOf InitializeProgram)
	'	thread.SetApartmentState(ApartmentState.STA)
	'	thread.Start()
	'	_startTimer.Stop()
	'	_startTimer.Dispose()
	'End Sub

	Private Shared Function InitializeProgram() As Boolean
        EventLogger.WriteToEventLog("Initializing Web Service")

        _log.Info("InitializeProgram: Thread Aparment State is: " & Threading.Thread.CurrentThread.GetApartmentState.ToString)

        _log.Info("Initializing Web Service...")

        Try
            InitializeWebService()
        Catch ex As Exception
            _log.Error("Web Service Initialization FAILED", ex)
            EventLogger.WriteToEventLog("Web Service Initialization FAILED: " & ex.ToString, EventLogEntryType.Error)
            Return False
        End Try

        EventLogger.WriteToEventLog("Web Service Initialization Success")
        _log.Info("Web Service Initialization Success")
        Return True
    End Function

    Private Shared Sub InitializeWebService()
		_log.Info("Creating WebServiceHost On http://localhost:8000")
		Dim theHost As New WebServiceHost(GetType(ServiceHost), New Uri("http://localhost:8000"))

		_log.Info("Adding Service Endpoint")
		Dim ep As ServiceEndpoint = theHost.AddServiceEndpoint(GetType(IServiceHost), New WebHttpBinding(), "")
		Dim stp As ServiceDebugBehavior = theHost.Description.Behaviors.Find(Of ServiceDebugBehavior)()

        stp.HttpHelpPageEnabled = True
        stp.IncludeExceptionDetailInFaults = True

        _log.Info("Opening Host")
        theHost.Open()
    End Sub

    Protected Overrides Sub OnStop()
        _log.Info("<<<< STOPPING mPosEngine SERVICE >>>>")

        Try
            if _mainThread isnot nothing andalso _mainThread.IsAlive Then _mainThread.Abort()
        Catch ex As Exception
            _log.Error("Can't abort thread. Exception: " + ex.Message, ex)
        End Try

        _log.Info("<<<< mPosEngine Service Stopped>>>>")
    End Sub

    Private Shared Sub InitializeLogging()
        Try
            'log4net.Config.XmlConfigurator.Configure(New FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DCS.mPosEngine.WebService.exe.config")))
            log4net.Config.XmlConfigurator.Configure() 'Usa el file especificado en assemblyinfo            
        Catch ex As Exception
            Throw New ApplicationException("Unable to initialize log4net logger: " & ex.Message)
        End Try
    End Sub

    '<MTAThread()> _
    '<System.Diagnostics.DebuggerNonUserCode()> _
    <STAThread()>
    Shared Sub Main()
        Dim servicesToRun() As System.ServiceProcess.ServiceBase

        ' More than one NT Service may run within the same process. To add
        ' another service to this process, change the following line to
        ' create a second service object. For example,
        '
        '   ServicesToRun = New System.ServiceProcess.ServiceBase () {New Service1, New MySecondUserService}
        '                

        Try
            InitializeLogging()
        Catch ex As Exception
            Exit Sub
        End Try

        _log.Info("<<<< Starting mPosEngine v" & Assembly.GetExecutingAssembly().GetName().Version.ToString() & " >>>>")

        _log.Info("Thread Aparment State is: " & Threading.Thread.CurrentThread.GetApartmentState.ToString)

        If Environment.UserInteractive Then
            Dim args As String() = Environment.GetCommandLineArgs

            If Not args Is Nothing AndAlso args.Length = 2 AndAlso args(1).Length > 1 AndAlso (args(1)(0) = "-" Or args(1)(0) = "/") Then
                Select Case args(1).Substring(1).ToLower
                    Case "install", "i"
                        Console.WriteLine("Installing mPosEngine Service...")
                        EventLogger.WriteToEventLog("Running mPosEngine in service install mode")
                        If SelfInstaller.InstallMe() Then
                            Console.WriteLine("Service Installed Successfully")
                        End If
                    Case "uninstall", "u"
                        EventLogger.WriteToEventLog("Running mPosEngine in service uninstall mode")
                        Console.WriteLine("Uninstalling mPosEngine Service...")
                        If SelfInstaller.UninstallMe() Then
                            Console.WriteLine("Service Uninstalled Successfully")
                        End If
                    Case "console", "c"
                        EventLogger.WriteToEventLog("Running mPosEngine in console mode")
                        Console.WriteLine("Initializing mPosEngine...")
                        If InitializeProgram() Then
                            AddHandler Console.CancelKeyPress, AddressOf BreakProgram
                            Console.WriteLine("mPosEngine is running")
                            Console.WriteLine("OS {0}, Process {1}", System.Environment.Is64BitOperatingSystem, System.Environment.Is64BitProcess)
                            Application.Run()
                        Else
                            'MsgBox("There was an error during program startup. Read log for details.", vbCritical)
                            EventLogger.WriteToEventLog("Program initialization failed", EventLogEntryType.Error)
                            Console.WriteLine("There was an error during program startup. Read log for details.")
                        End If
                    Case Else
                        EventLogger.WriteToEventLog("mPosEngine executed with invalid commandline parameters", EventLogEntryType.Warning)
                        Console.WriteLine("mPosEngine is a service. Valid Command Line Attributes are '-i', '-u' or '-c'")
                End Select
            Else
                EventLogger.WriteToEventLog("mPosEngine executed with invalid commandline parameters", EventLogEntryType.Warning)
                Console.WriteLine("mPosEngine is a service. Valid Command Line Attributes are '-i', '-u' or '-c'")
            End If
        Else
            '_log.Info("Sleeping 15 seconds to wait for debugger")
            'Threading.Thread.Sleep(15000)
            _log.Info("Starting mPosEngine as a service")
            servicesToRun = New System.ServiceProcess.ServiceBase() {New DcsMposEngineWebService}
            Run(servicesToRun)
        End If
    End Sub

	Private Shared Sub BreakProgram()
		EventLogger.WriteToEventLog("Program break detected while running in console mode, stopping program")
		Console.WriteLine("CTRL-C or CTRL-BREAK Detected")
		Console.WriteLine("Stopping mPosEngine...")
		'StopProgram()
		Console.WriteLine("Program Stopped")
	End Sub
End Class