using System.Collections.Generic;

namespace KantechGatewayApp.KantechGatewayApp.Jobs
{
    public class VendorDailyProcessor : BaseProcessor
    {
        protected override void HandleRow(Dictionary<string, string> row)
        {
            Infrastructure.KantechJobApi.TriggerJobApi(_jobKey, row);
            Infrastructure.Logger.Info($"[VendorDaily] emp={row.GetValueOrDefault("employee ID")} card={row.GetValueOrDefault("Card ID")} -> daily access set", _logSub);
        }
    }
}