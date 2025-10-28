using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KantechGatewayApp.KantechGatewayApp.Jobs
{
    public abstract class BaseProcessor : IJobProcessor
    {

        public string Key { get; private set; }
        public string ScheduleName { get; private set; }
        public DateTime LastRunUtc { get; private set; }
        public string LastResult { get; private set; }
        public string Status { get; private set; }

        private string _source;
        private string _target;
        protected string _logSub;
        private string _schedule;
        private string[] _requiredFields;
        private string _jobKey;

        public virtual void Configure(string jobKey)
        {
            string inbound = Infrastructure.JobManager.Config("Root.Inbound");
            string completed = Infrastructure.JobManager.Config("Root.Completed");
            string logs = Infrastructure.JobManager.Config("Root.Logs");
            _source = Infrastructure.FileUtil.Combine(inbound, Infrastructure.JobManager.Config($"Job.{jobKey}.Source"));
            _target = Infrastructure.FileUtil.Combine(completed, Infrastructure.JobManager.Config($"Job.{jobKey}.Target"));
            _logSub = Infrastructure.JobManager.Config($"Job.{jobKey}.LogSub", jobKey);
            _schedule = Infrastructure.JobManager.Config($"Job.{jobKey}.Schedule", "TwiceDaily");
            _requiredFields = Infrastructure.JobManager.Config($"Job.{jobKey}.RequiredFields", "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();

            Infrastructure.FileUtil.EnsureDir(_source);
            Infrastructure.FileUtil.EnsureDir(_target);
            Infrastructure.FileUtil.EnsureDir(Path.Combine(logs, _logSub));
            ScheduleName = _schedule;
            _jobKey = jobKey;
        }

        protected virtual bool IsTimeToRun()
        {
            var now = DateTime.Now;
            if (_schedule.Equals("TwiceDaily", StringComparison.OrdinalIgnoreCase))
            {
                string[] times = Infrastructure.JobManager.Config("Schedule.TwiceDailyTimes", "09:00,21:00").Split(',');
                return times.Any(t => SameMinute(now, t));
            }
            else if (_schedule.Equals("Daily", StringComparison.OrdinalIgnoreCase))
            {
                string t = Infrastructure.JobManager.Config("Schedule.DailyTime", "02:00");
                return SameMinute(now, t);
            }
            return false;
        }

        private static bool SameMinute(DateTime now, string hhmm)
        {
            string[] parts = hhmm.Split(':');
            int h = int.Parse(parts[0]);
            int m = int.Parse(parts[1]);
            return now.Hour == h && now.Minute == m;
        }

        public void MaybeRunBySchedule()
        {
            if (IsTimeToRun())
                RunOnce();
        }

        public void RunOnce()
        {
            try
            {
                LastRunUtc = DateTime.UtcNow;
                string pattern = Infrastructure.JobManager.Config("File.Pattern", "*.csv");
                string[] files = Directory.GetFiles(_source, pattern);
                int processedCount = 0;
                string status = "";
                if (files.Length == 0)
                {
                    LastResult = "No files";
                    return;
                }

                foreach (var f in files)
                {
                    try
                    {
                        ProcessOneFile(f);
                        processedCount += 1;
                        // Update progress status after each file
                        status = $"{processedCount} out of {files.Length} processed";
                    }
                    catch (Exception ex)
                    {
                        Infrastructure.Logger.Error($"[{Key}] failed to process file {f}", ex, _logSub);
                    }
                }
                LastResult = $"Processed {files.Length} file(s)";
                Status = status;
            }
            catch (Exception ex)
            {
                Infrastructure.Logger.Error($"[{Key}] run failed", ex, _logSub);
                LastResult = $"Error: {ex.Message}";
            }
        }

        private void ProcessOneFile(string filePath)
        {
            char delim = Convert.ToChar(Infrastructure.JobManager.Config("Csv.Delimiter", ","));
            bool hasHeader = bool.Parse(Infrastructure.JobManager.Config("Csv.HasHeader", "true"));
            var rows = Infrastructure.CsvUtil.ReadRows(filePath, delim, hasHeader);

            foreach (var rf in _requiredFields)
            {
                if (rows.Count > 0 && !rows[0].ContainsKey(rf))
                {
                    throw new ApplicationException($"Missing required field '{rf}' in {Path.GetFileName(filePath)}");
                }
            }

            int ok = 0;
            int err = 0;
            foreach (var r in rows)
            {
                try
                {
                    HandleRow(r);
                    ok += 1;
                }
                catch (Exception ex)
                {
                    err += 1;
                    Infrastructure.Logger.Error($"[{Key}] row failed: {string.Join(", ", r.Select(kv => kv.Key + "=" + kv.Value))}", ex, _logSub);
                }
            }

            if (err == 0)
            {
                Infrastructure.FileUtil.SafeMove(filePath, _target, _jobKey);
            }
            else
            {
                string errDir = Infrastructure.JobManager.Config("Root.Error");
                Infrastructure.FileUtil.SafeMove(filePath, Infrastructure.FileUtil.Combine(errDir, Key), _jobKey);
            }

            Infrastructure.Logger.Info($"[{Key}] {Path.GetFileName(filePath)} -> OK:{ok} ERR:{err}", _logSub);
        }

        protected abstract void HandleRow(Dictionary<string, string> row);
    }
}