using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace KantechGatewayApp.KantechGatewayApp.Infrastructure
{
    public class JobManager
    {
        private readonly Dictionary<string, Jobs.IJobProcessor> _jobs = new Dictionary<string, Jobs.IJobProcessor>(StringComparer.OrdinalIgnoreCase);
        private SimpleScheduler _scheduler;

        public IEnumerable<string> JobKeys
        {
            get
            {
                return _jobs.Keys;
            }
        }

        public void InitializeFromConfig()
        {
            AddJob("Deactivate", new Jobs.DeactivateProcessor());
            AddJob("Reactivate", new Jobs.ReactivateProcessor());
            AddJob("Resignee", new Jobs.ResigneeProcessor());
            AddJob("ProfileUpdate", new Jobs.ProfileUpdateProcessor());
            AddJob("AccessUpdate", new Jobs.AccessUpdateProcessor());
            AddJob("NewProfile", new Jobs.NewProfileProcessor());
            AddJob("NewAccess", new Jobs.NewAccessProcessor());
            AddJob("VendorDaily", new Jobs.VendorDailyProcessor());

            int interval = int.Parse(Config("Schedule.IntervalSec", "300"));
            _scheduler = new SimpleScheduler(this, interval);
        }

        private void AddJob(string key, Jobs.IJobProcessor proc)
        {
            proc.Configure(key);
            _jobs[key] = proc;
        }

        public void Start()
        {
            _scheduler.Start();
        }
        public void Stop()
        {
            _scheduler.Stop();
        }

        public void RunNow(string jobKey)
        {
            if (_jobs.ContainsKey(jobKey))
                _jobs[jobKey].RunOnce();
        }

        public void MaybeRunScheduled()
        {
            foreach (var kv in _jobs)
                kv.Value.MaybeRunBySchedule();
        }

        public DataTable GetStatusTable()
        {
            var t = new DataTable();
            t.Columns.Add("Job");
            t.Columns.Add("LastRun");
            t.Columns.Add("LastResult");
            t.Columns.Add("Schedule");
            t.Columns.Add("Status");
            foreach (var kv in _jobs)
            {
                var r = t.NewRow();
                r["Job"] = kv.Key;
                r["LastRun"] = kv.Value.LastRunUtc == DateTime.MinValue ? "" : kv.Value.LastRunUtc.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                r["LastResult"] = kv.Value.LastResult;
                r["Schedule"] = kv.Value.ScheduleName;
                r["Status"] = kv.Value.Status;
                t.Rows.Add(r);
            }
            return t;
        }

        internal static string Config(string key, string def = "")
        {
            string v = ConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(v) ? def : v;
        }
    }
}