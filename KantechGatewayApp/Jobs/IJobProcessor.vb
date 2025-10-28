Namespace KantechGatewayApp.Jobs
    Public Interface IJobProcessor
        ReadOnly Property Key As String
        ReadOnly Property ScheduleName As String
        ReadOnly Property LastRunUtc As Date
        ReadOnly Property LastResult As String
        ReadOnly Property Status As String

        Sub Configure(jobKey As String)
        Sub MaybeRunBySchedule()
        Sub RunOnce()
    End Interface
End Namespace