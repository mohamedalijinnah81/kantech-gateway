Namespace KantechGatewayApp.Jobs
    Public Class VendorDailyProcessor
        Inherits BaseProcessor
        Protected Overrides Sub HandleRow(row As Dictionary(Of String, String))
            Infrastructure.Logger.Info($"[VendorDaily] emp={row.GetValueOrDefault("employee ID")} card={row.GetValueOrDefault("Card ID")} -> daily access set", _logSub)
        End Sub
    End Class
End Namespace