using System;
using System.Drawing;
using System.Windows.Forms;

namespace KantechGatewayApp
{

    public partial class MainForm
    {
        private KantechGatewayApp.Infrastructure.JobManager _mgr;
        private bool _isRunning = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = "KantechGatewayApp";
            _mgr = new KantechGatewayApp.Infrastructure.JobManager();
            _mgr.InitializeFromConfig();

            // Start automatically on launch
            StartScheduler();

            RefreshGrid();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_isRunning)
            {
                StopScheduler();
            }
            else
            {
                StartScheduler();
            }
        }

        // Reusable start logic
        private void StartScheduler()
        {
            try
            {
                _mgr.Start();
                _isRunning = true;
                btnStart.Text = "Stop";
                btnStart.BackColor = Color.Red;
                KantechGatewayApp.Infrastructure.Logger.Info("Scheduler started automatically");
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while starting: " + ex.Message);
            }
        }

        // Reusable stop logic
        private void StopScheduler()
        {
            try
            {
                _mgr.Stop();
                _isRunning = false;
                btnStart.Text = "Start";
                btnStart.BackColor = Color.Green;
                KantechGatewayApp.Infrastructure.Logger.Info("Scheduler stopped");
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while stopping: " + ex.Message);
            }
        }

        private void btnRunNow_Click(object sender, EventArgs e)
        {
            // Check if scheduler is running
            if (!_isRunning)
            {
                MessageBox.Show("Please start the scheduler first.", "Scheduler Not Running", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string jobKey = lstJobs.SelectedItem as string;
            if (string.IsNullOrEmpty(jobKey))
            {
                MessageBox.Show("Select a job in the list.");
                return;
            }

            _mgr.RunNow(jobKey);
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            lstJobs.Items.Clear();
            foreach (var k in _mgr.JobKeys)
                lstJobs.Items.Add(k);
            grdStatus.DataSource = _mgr.GetStatusTable();
        }
    }
}