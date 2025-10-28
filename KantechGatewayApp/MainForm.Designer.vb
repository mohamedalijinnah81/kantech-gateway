Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.btnStop = New System.Windows.Forms.Button()
        Me.btnRunNow = New System.Windows.Forms.Button()
        Me.lstJobs = New System.Windows.Forms.ListBox()
        Me.grdStatus = New System.Windows.Forms.DataGridView()
        CType(Me.grdStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(12, 12)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(90, 30)
        Me.btnStart.TabIndex = 0
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'btnRunNow
        '
        Me.btnRunNow.Location = New System.Drawing.Point(204, 12)
        Me.btnRunNow.Name = "btnRunNow"
        Me.btnRunNow.Size = New System.Drawing.Size(90, 30)
        Me.btnRunNow.TabIndex = 2
        Me.btnRunNow.Text = "Run Now"
        Me.btnRunNow.UseVisualStyleBackColor = True
        '
        'lstJobs
        '
        Me.lstJobs.FormattingEnabled = True
        Me.lstJobs.ItemHeight = 15
        Me.lstJobs.Location = New System.Drawing.Point(12, 56)
        Me.lstJobs.Name = "lstJobs"
        Me.lstJobs.Size = New System.Drawing.Size(282, 214)
        Me.lstJobs.TabIndex = 3
        '
        'grdStatus
        '
        Me.grdStatus.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdStatus.Location = New System.Drawing.Point(300, 12)
        Me.grdStatus.Name = "grdStatus"
        Me.grdStatus.RowTemplate.Height = 25
        Me.grdStatus.Size = New System.Drawing.Size(548, 258)
        Me.grdStatus.TabIndex = 4
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(860, 282)
        Me.Controls.Add(Me.grdStatus)
        Me.Controls.Add(Me.lstJobs)
        Me.Controls.Add(Me.btnRunNow)
        Me.Controls.Add(Me.btnStart)
        Me.Name = "MainForm"
        Me.Text = "KantechGatewayApp"
        CType(Me.grdStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnStart As Button
    Friend WithEvents btnStop As Button
    Friend WithEvents btnRunNow As Button
    Friend WithEvents lstJobs As ListBox
    Friend WithEvents grdStatus As DataGridView
End Class