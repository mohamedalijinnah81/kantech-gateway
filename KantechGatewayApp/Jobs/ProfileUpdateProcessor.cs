using System.Collections.Generic;

namespace KantechGatewayApp.KantechGatewayApp.Jobs
{
    public class ProfileUpdateProcessor : BaseProcessor
    {
        protected override void HandleRow(Dictionary<string, string> row)
        {
            Infrastructure.KantechJobApi.TriggerJobApi(_jobKey, row);
            Infrastructure.Logger.Info($"[ProfileUpdate] emp={row.GetValueOrDefault("employee ID")} dept={row.GetValueOrDefault("dept")} -> profile updated", _logSub);
        }
    }
}