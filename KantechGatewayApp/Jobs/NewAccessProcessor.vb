Namespace KantechGatewayApp.Jobs
    Public Class NewAccessProcessor
        Inherits BaseProcessor
        Protected Overrides Sub HandleRow(row As Dictionary(Of String, String))
            Infrastructure.Logger.Info($"[NewAccess] emp={row.GetValueOrDefault("employee ID")} card={row.GetValueOrDefault("Card ID")} -> access created", _logSub)
        End Sub
    End Class
End Namespace