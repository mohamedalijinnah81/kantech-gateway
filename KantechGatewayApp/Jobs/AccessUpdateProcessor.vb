Namespace KantechGatewayApp.Jobs
    Public Class AccessUpdateProcessor
        Inherits BaseProcessor
        Protected Overrides Sub HandleRow(row As Dictionary(Of String, String))
            Infrastructure.Logger.Info($"[AccessUpdate] emp={row.GetValueOrDefault("employee ID")} card={row.GetValueOrDefault("Card ID")} level={row.GetValueOrDefault("access level")} site={row.GetValueOrDefault("site link name")} -> access updated", _logSub)
        End Sub
    End Class
End Namespace