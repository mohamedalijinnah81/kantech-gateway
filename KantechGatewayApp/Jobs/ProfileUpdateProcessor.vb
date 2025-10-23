Namespace KantechGatewayApp.Jobs
    Public Class ProfileUpdateProcessor
        Inherits BaseProcessor
        Protected Overrides Sub HandleRow(row As Dictionary(Of String, String))
            Infrastructure.Logger.Info($"[ProfileUpdate] emp={row.GetValueOrDefault("employee ID")} dept={row.GetValueOrDefault("dept")} -> profile updated")
        End Sub
    End Class
End Namespace