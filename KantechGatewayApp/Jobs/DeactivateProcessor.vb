Namespace KantechGatewayApp.Jobs
    Public Class DeactivateProcessor
        Inherits BaseProcessor
        Protected Overrides Sub HandleRow(row As Dictionary(Of String, String))
            Dim cardId = row.GetValueOrDefault("Card ID")
            Dim empId = row.GetValueOrDefault("employee ID")
            Infrastructure.Logger.Info($"[Deactivate] card={cardId} emp={empId} -> deactivated", _logSub)
        End Sub
    End Class
End Namespace