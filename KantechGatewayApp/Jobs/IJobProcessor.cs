using System;

namespace KantechGatewayApp.KantechGatewayApp.Jobs
{
    public interface IJobProcessor
    {
        string Key { get; }
        string ScheduleName { get; }
        DateTime LastRunUtc { get; }
        string LastResult { get; }
        string Status { get; }

        void Configure(string jobKey);
        void MaybeRunBySchedule();
        void RunOnce();
    }
}