using EntraPass.Util; // make sure the EntraPass SDK or DLL reference is added
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Xml.Serialization;

namespace KantechGatewayApp.KantechGatewayApp.Infrastructure
{
    public static class KantechApi
    {
        private static string ServerIP => JobManager.Config("Kantech.ServerIP");
        private static string ServerPort => JobManager.Config("Kantech.ServerPort");
        private static string ServerUsername => JobManager.Config("Kantech.ServerUsername");
        private static string ServerPassword => JobManager.Config("Kantech.ServerPassword");
        private static string ConnectPartner => JobManager.Config("Kantech.ConnectPartner");

        public static void LoginKantech()
        {
            try
            {
                var encrypt = new EncryptUtil();
                var newGuid = Guid.NewGuid();

                // --- Encrypt fields like VB code ---
                string encryptUsername = HttpUtility.UrlEncode(encrypt.Encrypt(ServerUsername.ToLower(), ServerPassword));
                string encryptPassword = HttpUtility.UrlEncode(encrypt.Encrypt(ServerPassword, ServerPassword));
                string encryptConnectPartner = HttpUtility.UrlEncode(encrypt.Encrypt($"{ConnectPartner},{newGuid}", ServerPassword));

                // --- Build request URL ---
                string urlPath = $"/SmartService/Login?userName={encryptUsername}&password={encryptPassword}&encrypted=1&ConnectedProgram={encryptConnectPartner}";
                string fullUrl = $"http://{ServerIP}:{ServerPort}{urlPath}";

                Logger.Info($"Attempting Kantech login: {fullUrl}", "Kantech");

                // --- Perform HTTP GET ---
                string messageResult = GetHttpResponse(fullUrl);

                // --- Response XML cleaning logic (same as VB) ---
                if (!string.IsNullOrWhiteSpace(messageResult))
                {
                    messageResult = messageResult
                        .Replace(@"<StandardFault xmlns=""http://schemas.datacontract.org/2004/07/Kantech.SmartLinkSDK"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">", "")
                        .Replace(@"<?xml version=""1.0"" encoding=""utf-8""?>", "")
                        .Replace("<Operator>", "<OperatorData>")
                        .Replace("</Operator>", "</OperatorData>");
                }

                // --- Validate response ---
                if (messageResult.Contains("Error Login", StringComparison.OrdinalIgnoreCase))
                    throw new Exception("Login failed: invalid credentials or server error.");

                // --- Deserialize response XML into OperatorData ---
                var serializer = new XmlSerializer(typeof(OperatorData));
                using (var reader = new StringReader(messageResult))
                {
                    var operatorData = (OperatorData)serializer.Deserialize(reader);

                    if (string.IsNullOrEmpty(operatorData.SessionKey))
                        throw new Exception("Login failed: Missing SessionKey in response.");

                    // --- Assign session details globally ---
                    KantechSessionManager.SessionKey = operatorData.SessionKey;
                    KantechSessionManager.LastLoginTime = DateTime.Now;

                    Logger.Info($"Kantech login successful. SessionKey: {operatorData.SessionKey}", "Kantech");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"LoginKantech failed: {ex.Message}", ex, "Kantech");
                throw;
            }
        }

        private static string GetHttpResponse(string url)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                using (var response = request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                Logger.Error($"HTTP request failed: {ex.Message}, URL: {url}", ex, "Kantech");
                throw;
            }
        }
    }

    [Serializable]
    [XmlRoot("OperatorData")]
    public class OperatorData
    {
        [XmlElement("SessionKey")]
        public string SessionKey { get; set; }
    }
}
