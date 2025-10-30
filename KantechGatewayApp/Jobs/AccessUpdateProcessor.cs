using System.Collections.Generic;

namespace KantechGatewayApp.KantechGatewayApp.Jobs
{
    public class AccessUpdateProcessor : BaseProcessor
    {
        protected override void HandleRow(Dictionary<string, string> row)
        {
            Infrastructure.KantechJobApi.TriggerJobApi(_jobKey, row);
            Infrastructure.Logger.Info($"[AccessUpdate] emp={row.GetValueOrDefault("employee ID")} card={row.GetValueOrDefault("Card ID")} level={row.GetValueOrDefault("access level")} site={row.GetValueOrDefault("site link name")} -> access updated", _logSub);
        }
    }
}