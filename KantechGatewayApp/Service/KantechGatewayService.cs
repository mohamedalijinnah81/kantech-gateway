using System.ServiceProcess;

namespace KantechGatewayApp.KantechGatewayApp.Service
{
    public class KantechGatewayService : ServiceBase
    {

        private Infrastructure.JobManager _mgr;

        public KantechGatewayService()
        {
            ServiceName = "KantechGatewayApp";
            CanStop = true;
            CanPauseAndContinue = false;
            AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            Infrastructure.Logger.Info("Service starting...");
            _mgr = new Infrastructure.JobManager();
            _mgr.InitializeFromConfig();
            _mgr.Start();
            Infrastructure.Logger.Info("Service started.");
        }

        protected override void OnStop()
        {
            Infrastructure.Logger.Info("Service stopping...");
            _mgr?.Stop();
            Infrastructure.Logger.Info("Service stopped.");
        }
    }
}