using System.Collections.Generic;

namespace KantechGatewayApp.KantechGatewayApp.Jobs
{
    public class ReactivateProcessor : BaseProcessor
    {
        protected override void HandleRow(Dictionary<string, string> row)
        {
            Infrastructure.KantechJobApi.TriggerJobApi(_jobKey, row);
            Infrastructure.Logger.Info($"[Reactivate] card={row.GetValueOrDefault("Card ID")} emp={row.GetValueOrDefault("employee ID")} -> reactivated", _logSub);
        }
    }
}