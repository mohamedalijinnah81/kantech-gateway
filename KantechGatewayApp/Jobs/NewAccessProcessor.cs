using System.Collections.Generic;

namespace KantechGatewayApp.KantechGatewayApp.Jobs
{
    public class NewAccessProcessor : BaseProcessor
    {
        protected override void HandleRow(Dictionary<string, string> row)
        {
            Infrastructure.KantechJobApi.TriggerJobApi(_jobKey, row);
            Infrastructure.Logger.Info($"[NewAccess] emp={row.GetValueOrDefault("employee ID")} card={row.GetValueOrDefault("Card ID")} -> access created", _logSub);
        }
    }
}