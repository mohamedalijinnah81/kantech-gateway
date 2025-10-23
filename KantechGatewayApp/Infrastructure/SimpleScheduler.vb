Namespace KantechGatewayApp.Infrastructure
    Public Class SimpleScheduler
        Private ReadOnly _mgr As JobManager
        Private ReadOnly _timer As Timers.Timer

        Public Sub New(mgr As JobManager, intervalSec As Integer)
            _mgr = mgr
            _timer = New Timers.Timer(intervalSec * 1000)
            AddHandler _timer.Elapsed, AddressOf OnTick
            _timer.AutoReset = True
        End Sub

        Public Sub Start()
            _timer.Start()
        End Sub
        Public Sub [Stop]()
            _timer.Stop()
        End Sub

        Private Sub OnTick(sender As Object, e As Timers.ElapsedEventArgs)
            _mgr.MaybeRunScheduled()
        End Sub
    End Class
End Namespace