Imports System.IO
Namespace KantechGatewayApp.Infrastructure
    Public NotInheritable Class Logger
        Private Shared ReadOnly logRoot As String = IO.Path.GetFullPath(System.Configuration.ConfigurationManager.AppSettings("Root.Logs"))

        Private Shared Function LogPath(Optional subFolder As String = Nothing) As String
            Dim fname = If(Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings("Log.RollDaily")), $"app_{Date.Today:yyyyMMdd}.log", "app.log")
            Dim logDir = If(String.IsNullOrEmpty(subFolder), logRoot, Path.Combine(logRoot, subFolder))
            Directory.CreateDirectory(logDir)
            Return Path.Combine(logDir, fname)
        End Function

        Public Shared Sub Info(msg As String, Optional subFolder As String = Nothing)
            Write("INFO", msg, subFolder)
        End Sub
        Public Shared Sub Warn(msg As String, Optional subFolder As String = Nothing)
            Write("WARN", msg, subFolder)
        End Sub
        Public Shared Sub [Error](msg As String, Optional ex As Exception = Nothing, Optional subFolder As String = Nothing)
            Write("ERROR", If(ex Is Nothing, msg, $"{msg} :: {ex}"), subFolder)
        End Sub

        Private Shared Sub Write(level As String, msg As String, subFolder As String)
            Dim line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {msg}"
            SyncLock GetType(Logger)
                File.AppendAllText(LogPath(subFolder), line & Environment.NewLine)
            End SyncLock
        End Sub
    End Class
End Namespace