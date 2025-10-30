using System;

namespace KantechGatewayApp.KantechGatewayApp.Infrastructure
{
    public static class KantechSessionManager
    {
        public static string SessionKey { get; set; } = string.Empty;
        public static DateTime LastLoginTime { get; set; } = DateTime.MinValue;
    }
}

