Namespace KantechGatewayApp.Jobs
    Public Class ResigneeProcessor
        Inherits BaseProcessor
        Protected Overrides Sub HandleRow(row As Dictionary(Of String, String))
            Infrastructure.Logger.Info($"[Resignee] card={row.GetValueOrDefault("Card ID")} emp={row.GetValueOrDefault("employee ID")} -> access removed", _logSub)
        End Sub
    End Class
End Namespace