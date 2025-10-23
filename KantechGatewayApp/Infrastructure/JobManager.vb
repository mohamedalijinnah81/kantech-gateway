Imports System.Configuration
Imports System.Data

Namespace KantechGatewayApp.Infrastructure
    Public Class JobManager
        Private ReadOnly _jobs As New Dictionary(Of String, Jobs.IJobProcessor)(StringComparer.OrdinalIgnoreCase)
        Private _scheduler As SimpleScheduler

        Public ReadOnly Property JobKeys As IEnumerable(Of String)
            Get
                Return _jobs.Keys
            End Get
        End Property

        Public Sub InitializeFromConfig()
            AddJob("Deactivate", New Jobs.DeactivateProcessor())
            AddJob("Reactivate", New Jobs.ReactivateProcessor())
            AddJob("Resignee", New Jobs.ResigneeProcessor())
            AddJob("ProfileUpdate", New Jobs.ProfileUpdateProcessor())
            AddJob("AccessUpdate", New Jobs.AccessUpdateProcessor())
            AddJob("NewProfile", New Jobs.NewProfileProcessor())
            AddJob("NewAccess", New Jobs.NewAccessProcessor())
            AddJob("VendorDaily", New Jobs.VendorDailyProcessor())

            Dim interval = Integer.Parse(Config("Schedule.IntervalSec", "300"))
            _scheduler = New SimpleScheduler(Me, interval)
        End Sub

        Private Sub AddJob(key As String, proc As Jobs.IJobProcessor)
            proc.Configure(key)
            _jobs(key) = proc
        End Sub

        Public Sub Start()
            _scheduler.Start()
        End Sub
        Public Sub [Stop]()
            _scheduler.Stop()
        End Sub

        Public Sub RunNow(jobKey As String)
            If _jobs.ContainsKey(jobKey) Then _jobs(jobKey).RunOnce()
        End Sub

        Public Sub MaybeRunScheduled()
            For Each kv In _jobs
                kv.Value.MaybeRunBySchedule()
            Next
        End Sub

        Public Function GetStatusTable() As DataTable
            Dim t As New DataTable()
            t.Columns.Add("Job")
            t.Columns.Add("LastRun")
            t.Columns.Add("LastResult")
            t.Columns.Add("Schedule")
            For Each kv In _jobs
                Dim r = t.NewRow()
                r("Job") = kv.Key
                r("LastRun") = If(kv.Value.LastRunUtc = Date.MinValue, "", kv.Value.LastRunUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"))
                r("LastResult") = kv.Value.LastResult
                r("Schedule") = kv.Value.ScheduleName
                t.Rows.Add(r)
            Next
            Return t
        End Function

        Friend Shared Function Config(key As String, Optional def As String = "") As String
            Dim v = ConfigurationManager.AppSettings(key)
            Return If(String.IsNullOrWhiteSpace(v), def, v)
        End Function
    End Class
End Namespace