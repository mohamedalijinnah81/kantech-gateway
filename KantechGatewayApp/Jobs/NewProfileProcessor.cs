using System.Collections.Generic;

namespace KantechGatewayApp.KantechGatewayApp.Jobs
{
    public class NewProfileProcessor : BaseProcessor
    {
        protected override void HandleRow(Dictionary<string, string> row)
        {
            Infrastructure.Logger.Info($"[NewProfile] emp={row.GetValueOrDefault("employee ID")} name={row.GetValueOrDefault("name")} -> profile created", _logSub);
        }
    }
}