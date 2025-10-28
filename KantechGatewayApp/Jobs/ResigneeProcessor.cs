using System.Collections.Generic;

namespace KantechGatewayApp.KantechGatewayApp.Jobs
{
    public class ResigneeProcessor : BaseProcessor
    {
        protected override void HandleRow(Dictionary<string, string> row)
        {
            Infrastructure.Logger.Info($"[Resignee] card={row.GetValueOrDefault("Card ID")} emp={row.GetValueOrDefault("employee ID")} -> access removed", _logSub);
        }
    }
}