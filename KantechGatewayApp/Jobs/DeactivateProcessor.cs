using System.Collections.Generic;

namespace KantechGatewayApp.KantechGatewayApp.Jobs
{
    public class DeactivateProcessor : BaseProcessor
    {
        protected override void HandleRow(Dictionary<string, string> row)
        {
            string cardId = row.GetValueOrDefault("Card ID");
            string empId = row.GetValueOrDefault("employee ID");

            Infrastructure.KantechJobApi.TriggerJobApi(_jobKey, row);
            Infrastructure.Logger.Info($"[Deactivate] card={cardId} emp={empId} -> deactivated", _logSub);
        }
    }
}