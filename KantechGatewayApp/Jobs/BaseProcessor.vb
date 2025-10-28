Imports System.IO
Imports KantechGatewayApp.Infrastructure

Namespace KantechGatewayApp.Jobs
    Public MustInherit Class BaseProcessor
        Implements IJobProcessor

        Public ReadOnly Property Key As String Implements IJobProcessor.Key
        Public ReadOnly Property ScheduleName As String Implements IJobProcessor.ScheduleName
        Public ReadOnly Property LastRunUtc As Date Implements IJobProcessor.LastRunUtc
        Public ReadOnly Property LastResult As String Implements IJobProcessor.LastResult
        Public ReadOnly Property Status As String Implements IJobProcessor.Status

        Private _source As String
        Private _target As String
        Protected _logSub As String
        Private _schedule As String
        Private _requiredFields As String()
        Private _jobKey As String

        Public Overridable Sub Configure(jobKey As String) Implements IJobProcessor.Configure
            Dim inbound = Infrastructure.JobManager.Config("Root.Inbound")
            Dim completed = Infrastructure.JobManager.Config("Root.Completed")
            Dim logs = Infrastructure.JobManager.Config("Root.Logs")
            _source = KantechGatewayApp.Infrastructure.FileUtil.Combine(inbound, Infrastructure.JobManager.Config($"Job.{jobKey}.Source"))
            _target = KantechGatewayApp.Infrastructure.FileUtil.Combine(completed, Infrastructure.JobManager.Config($"Job.{jobKey}.Target"))
            _logSub = Infrastructure.JobManager.Config($"Job.{jobKey}.LogSub", jobKey)
            _schedule = Infrastructure.JobManager.Config($"Job.{jobKey}.Schedule", "TwiceDaily")
            _requiredFields = Infrastructure.JobManager.Config($"Job.{jobKey}.RequiredFields", "") _
                              .Split({","c}, StringSplitOptions.RemoveEmptyEntries) _
                              .Select(Function(s) s.Trim()).ToArray()
            KantechGatewayApp.Infrastructure.FileUtil.EnsureDir(_source) : KantechGatewayApp.Infrastructure.FileUtil.EnsureDir(_target) : KantechGatewayApp.Infrastructure.FileUtil.EnsureDir(Path.Combine(logs, _logSub))
            _ScheduleName = _schedule
            _jobKey = jobKey
        End Sub

        Protected Overridable Function IsTimeToRun() As Boolean
            Dim now = DateTime.Now
            If _schedule.Equals("TwiceDaily", StringComparison.OrdinalIgnoreCase) Then
                Dim times = Infrastructure.JobManager.Config("Schedule.TwiceDailyTimes", "09:00,21:00").Split(","c)
                Return times.Any(Function(t) SameMinute(now, t))
            ElseIf _schedule.Equals("Daily", StringComparison.OrdinalIgnoreCase) Then
                Dim t = Infrastructure.JobManager.Config("Schedule.DailyTime", "02:00")
                Return SameMinute(now, t)
            End If
            Return False
        End Function

        Private Shared Function SameMinute(now As DateTime, hhmm As String) As Boolean
            Dim parts = hhmm.Split(":"c)
            Dim h = Integer.Parse(parts(0)) : Dim m = Integer.Parse(parts(1))
            Return now.Hour = h AndAlso now.Minute = m
        End Function

        Public Sub MaybeRunBySchedule() Implements IJobProcessor.MaybeRunBySchedule
            If IsTimeToRun() Then RunOnce()
        End Sub

        Public Sub RunOnce() Implements IJobProcessor.RunOnce
            Try
                _LastRunUtc = Date.UtcNow
                Dim pattern = Infrastructure.JobManager.Config("File.Pattern", "*.csv")
                Dim files = Directory.GetFiles(_source, pattern)
                Dim processedCount As Integer = 0
                Dim status As String = ""
                If files.Length = 0 Then
                    _LastResult = "No files"
                    Return
                End If

                For Each f In files
                    Try
                        ProcessOneFile(f)
                        processedCount += 1
                        ' Update progress status after each file
                        status = $"{processedCount} out of {files.Length} processed"
                    Catch ex As Exception
                        Infrastructure.Logger.Error($"[{Key}] failed to process file {f}", ex, _logSub)
                    End Try
                Next
                _LastResult = $"Processed {files.Length} file(s)"
                _Status = status
            Catch ex As Exception
                Infrastructure.Logger.Error($"[{Key}] run failed", ex, _logSub)
                _LastResult = $"Error: {ex.Message}"
            End Try
        End Sub

        Private Sub ProcessOneFile(filePath As String)
            Dim delim = Convert.ToChar(Infrastructure.JobManager.Config("Csv.Delimiter", ","))
            Dim hasHeader = Boolean.Parse(Infrastructure.JobManager.Config("Csv.HasHeader", "true"))
            Dim rows = Infrastructure.CsvUtil.ReadRows(filePath, delim, hasHeader)

            For Each rf In _requiredFields
                If rows.Count > 0 AndAlso Not rows(0).ContainsKey(rf) Then
                    Throw New ApplicationException($"Missing required field '{rf}' in {Path.GetFileName(filePath)}")
                End If
            Next

            Dim ok As Integer = 0, err As Integer = 0
            For Each r In rows
                Try
                    HandleRow(r)
                    ok += 1
                Catch ex As Exception
                    err += 1
                    Infrastructure.Logger.Error($"[{Key}] row failed: {String.Join(", ", r.Select(Function(kv) kv.Key & "=" & kv.Value))}", ex, _logSub)
                End Try
            Next

            If err = 0 Then
                KantechGatewayApp.Infrastructure.FileUtil.SafeMove(filePath, _target, _jobKey)
            Else
                Dim errDir = Infrastructure.JobManager.Config("Root.Error")
                KantechGatewayApp.Infrastructure.FileUtil.SafeMove(filePath, KantechGatewayApp.Infrastructure.FileUtil.Combine(errDir, Key), _jobKey)
            End If

            Infrastructure.Logger.Info($"[{Key}] {Path.GetFileName(filePath)} -> OK:{ok} ERR:{err}", _logSub)
        End Sub

        Protected MustOverride Sub HandleRow(row As Dictionary(Of String, String))
    End Class
End Namespace