using KantechGatewayApp.KantechGatewayApp.Infrastructure;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;

namespace KantechGatewayApp.KantechGatewayApp.Infrastructure
{
    public static class KantechJobApi
    {
        public static void TriggerJobApi(string jobKey, Dictionary<string, string> rowData)
        {
            // Check session validity
            var minutesSinceLogin = (DateTime.Now - KantechSessionManager.LastLoginTime).TotalMinutes;
            if (string.IsNullOrEmpty(KantechSessionManager.SessionKey) || minutesSinceLogin > 5)
            {
                KantechApi.LoginKantech(); // Refresh session
            }

            string sessionKey = KantechSessionManager.SessionKey;
            string endpoint = GetEndpointForJob(jobKey);
            if (string.IsNullOrEmpty(endpoint))
            {
                Logger.Error($"Unknown jobKey '{jobKey}'", null, "Kantech");
                return;
            }

            try
            {
                var client = new RestClient(JobManager.Config("Kantech.Api.BaseUrl"));
                var request = new RestRequest(endpoint, Method.Post);
                request.AddHeader("Authorization", $"Bearer {sessionKey}");
                request.AddJsonBody(rowData);

                var response = client.Execute(request);
                if (!response.IsSuccessful)
                {
                    Logger.Error($"[{jobKey}] API failed: {response.Content}", null, "Kantech");
                }
                else
                {
                    Logger.Info($"[{jobKey}] API Success: {response.Content}", "Kantech");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"[{jobKey}] API call failed", ex, "Kantech");
            }
        }

        private static string GetEndpointForJob(string jobKey)
        {
            switch (jobKey)
            {
                case "Deactivate": return JobManager.Config("Kantech.Api.DeactivateEndpoint");
                case "Reactivate": return JobManager.Config("Kantech.Api.ReactivateEndpoint");
                case "Resignee": return JobManager.Config("Kantech.Api.ResigneeEndpoint");
                case "NewAccess": return JobManager.Config("Kantech.Api.NewAccessEndpoint");
                case "NewProfile": return JobManager.Config("Kantech.Api.NewProfileEndpoint");
                case "ProfileUpdate": return JobManager.Config("Kantech.Api.ProfileUpdateEndpoint");
                case "VendorDaily": return JobManager.Config("Kantech.Api.VendorDailyEndpoint");
                case "AccessUpdate": return JobManager.Config("Kantech.Api.AccessUpdateEndpoint");
                default: return null;
            }
        }
    }
}
