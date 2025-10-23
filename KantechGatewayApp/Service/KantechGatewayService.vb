Imports System.ServiceProcess
Imports KantechGatewayApp.Infrastructure

Namespace KantechGatewayApp.Service
    Public Class KantechGatewayService
        Inherits ServiceBase

        Private _mgr As Infrastructure.JobManager

        Public Sub New()
            ServiceName = "KantechGatewayApp"
            CanStop = True : CanPauseAndContinue = False : AutoLog = True
        End Sub

        Protected Overrides Sub OnStart(args() As String)
            Infrastructure.Logger.Info("Service starting...")
            _mgr = New Infrastructure.JobManager()
            _mgr.InitializeFromConfig()
            _mgr.Start()
            Infrastructure.Logger.Info("Service started.")
        End Sub

        Protected Overrides Sub OnStop()
            Infrastructure.Logger.Info("Service stopping...")
            _mgr?.[Stop]()
            Infrastructure.Logger.Info("Service stopped.")
        End Sub
    End Class
End Namespace