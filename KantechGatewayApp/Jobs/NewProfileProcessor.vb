Namespace KantechGatewayApp.Jobs
    Public Class NewProfileProcessor
        Inherits BaseProcessor
        Protected Overrides Sub HandleRow(row As Dictionary(Of String, String))
            Infrastructure.Logger.Info($"[NewProfile] emp={row.GetValueOrDefault("employee ID")} name={row.GetValueOrDefault("name")} -> profile created", _logSub)
        End Sub
    End Class
End Namespace