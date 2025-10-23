Imports System.ComponentModel
Imports KantechGatewayApp.Infrastructure
Imports KantechGatewayApp.Jobs
Imports System.Windows.Forms

Public Class MainForm
    Private _mgr As KantechGatewayApp.Infrastructure.JobManager

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "KantechGatewayApp"
        _mgr = New KantechGatewayApp.Infrastructure.JobManager()
        _mgr.InitializeFromConfig()
        RefreshGrid()
    End Sub

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        _mgr.Start()
        KantechGatewayApp.Infrastructure.Logger.Info("Scheduler started")
        RefreshGrid()
    End Sub

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        _mgr.[Stop]()
        KantechGatewayApp.Infrastructure.Logger.Info("Scheduler stopped")
        RefreshGrid()
    End Sub

    Private Sub btnRunNow_Click(sender As Object, e As EventArgs) Handles btnRunNow.Click
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