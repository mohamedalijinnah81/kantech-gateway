using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace KantechGatewayApp
{

    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class MainForm : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            btnStart = new Button();
            btnStart.Click += new EventHandler(btnStart_Click);
            btnStop = new Button();
            btnRunNow = new Button();
            btnRunNow.Click += new EventHandler(btnRunNow_Click);
            lstJobs = new ListBox();
            grdStatus = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)grdStatus).BeginInit();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(12, 12);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(90, 30);
            btnStart.TabIndex = 0;
            btnStart.Text = "Start";
            btnStart.ForeColor = Color.White;
            btnStart.UseVisualStyleBackColor = true;
            // 
            // btnRunNow
            // 
            btnRunNow.Location = new Point(204, 12);
            btnRunNow.Name = "btnRunNow";
            btnRunNow.Size = new Size(90, 30);
            btnRunNow.TabIndex = 2;
            btnRunNow.Text = "Run Now";
            btnRunNow.ForeColor = Color.White;
            btnRunNow.BackColor = Color.RoyalBlue;
            btnRunNow.UseVisualStyleBackColor = false;
            // 
            // lstJobs
            // 
            lstJobs.FormattingEnabled = true;
            lstJobs.ItemHeight = 15;
            lstJobs.Location = new Point(12, 56);
            lstJobs.Name = "lstJobs";
            lstJobs.Size = new Size(282, 214);
            lstJobs.TabIndex = 3;
            // 
            // grdStatus
            // 
            grdStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            grdStatus.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grdStatus.Location = new Point(300, 12);
            grdStatus.Name = "grdStatus";
            grdStatus.RowTemplate.Height = 25;
            grdStatus.Size = new Size(548, 258);
            grdStatus.TabIndex = 4;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7.0f, 15.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(860, 282);
            Controls.Add(grdStatus);
            Controls.Add(lstJobs);
            Controls.Add(btnRunNow);
            Controls.Add(btnStart);
            Name = "MainForm";
            Text = "KantechGatewayApp";
            ((System.ComponentModel.ISupportInitialize)grdStatus).EndInit();
            Load += new EventHandler(MainForm_Load);
            ResumeLayout(false);

        }

        internal Button btnStart;
        internal Button btnStop;
        internal Button btnRunNow;
        internal ListBox lstJobs;
        internal DataGridView grdStatus;
    }
}