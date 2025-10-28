
namespace KantechGatewayApp.KantechGatewayApp.Infrastructure
{
    public class SimpleScheduler
    {
        private readonly JobManager _mgr;
        private readonly System.Timers.Timer _timer;

        public SimpleScheduler(JobManager mgr, int intervalSec)
        {
            _mgr = mgr;
            _timer = new System.Timers.Timer(intervalSec * 1000);
            _timer.Elapsed += OnTick;
            _timer.AutoReset = true;
        }

        public void Start()
        {
            _timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
        }

        private void OnTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            _mgr.MaybeRunScheduled();
        }
    }
}