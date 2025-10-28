Imports System.ComponentModel
Imports KantechGatewayApp.Infrastructure
Imports KantechGatewayApp.Jobs
Imports System.Windows.Forms

Public Class MainForm
    Private _mgr As KantechGatewayApp.Infrastructure.JobManager
    Private _isRunning As Boolean = False

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "KantechGatewayApp"
        _mgr = New KantechGatewayApp.Infrastructure.JobManager()
        _mgr.InitializeFromConfig()

        ' Start automatically on launch
        StartScheduler()

        RefreshGrid()
    End Sub

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        If _isRunning Then
            StopScheduler()
        Else
            StartScheduler()
        End If
    End Sub

    ' Reusable start logic
    Private Sub StartScheduler()
        Try
            _mgr.Start()
            _isRunning = True
            btnStart.Text = "Stop"
            KantechGatewayApp.Infrastructure.Logger.Info("Scheduler started automatically")
            RefreshGrid()
        Catch ex As Exception
            MessageBox.Show("Error while starting: " & ex.Message)
        End Try
    End Sub

    ' Reusable stop logic
    Private Sub StopScheduler()
        Try
            _mgr.Stop()
            _isRunning = False
            btnStart.Text = "Start"
            KantechGatewayApp.Infrastructure.Logger.Info("Scheduler stopped")
            RefreshGrid()
        Catch ex As Exception
            MessageBox.Show("Error while stopping: " & ex.Message)
        End Try
    End Sub

    Private Sub btnRunNow_Click(sender As Object, e As EventArgs) Handles btnRunNow.Click
        ' Check if scheduler is running
        If Not _isRunning Then
            MessageBox.Show("Please start the scheduler first.", "Scheduler Not Running", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim jobKey = TryCast(lstJobs.SelectedItem, String)
        If String.IsNullOrEmpty(jobKey) Then
            MessageBox.Show("Select a job in the list.")
            Return
        End If

        _mgr.RunNow(jobKey)
        RefreshGrid()
    End Sub

    Private Sub RefreshGrid()
        lstJobs.Items.Clear()
        For Each k In _mgr.JobKeys
            lstJobs.Items.Add(k)
        Next
        grdStatus.DataSource = _mgr.GetStatusTable()
    End Sub
End Class
