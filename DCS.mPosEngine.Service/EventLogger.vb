Public Class EventLogger
    '*************************************************************
    'NAME:          WriteToEventLog
    'PURPOSE:       Write to Event Log
    'PARAMETERS:    Entry - Value to Write
    '               AppName - Name of Client Application. Needed 
    '               because before writing to event log, you must 
    '               have a named EventLog source. 
    '               EventType - Entry Type, from EventLogEntryType 
    '               Structure e.g., EventLogEntryType.Warning, 
    '               EventLogEntryType.Error
    '               LogNam1e: Name of Log (System, Application; 
    '               Security is read-only) If you 
    '               specify a non-existent log, the log will be
    '               created
    'RETURNS:       True if successful
    '*************************************************************
    Public Shared Function WriteToEventLog(ByVal entry As String, Optional ByVal eventType As EventLogEntryType = EventLogEntryType.Information) As Boolean

        Dim objEventLog As New EventLog

        Try

            'Register the Application as an Event Source
            If Not EventLog.SourceExists("SacoaDCS") Then
                EventLog.CreateEventSource("SacoaDCS", "mPosEngine")
            End If

            'log the entry
            objEventLog.Source = "SacoaDCS"
            objEventLog.WriteEntry(entry, eventType)

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function
End Class
