Namespace KantechGatewayApp.Jobs
    Public Class ReactivateProcessor
        Inherits BaseProcessor
        Protected Overrides Sub HandleRow(row As Dictionary(Of String, String))
            Infrastructure.Logger.Info($"[Reactivate] card={row.GetValueOrDefault("Card ID")} emp={row.GetValueOrDefault("employee ID")} -> reactivated")
        End Sub
    End Class
End Namespace