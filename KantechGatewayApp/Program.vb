Imports System.Configuration
Imports System.ServiceProcess
Imports System.Windows.Forms

Namespace KantechGatewayApp
    Module Program
        <STAThread()>
        Sub Main()
#If DEBUG Then
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Application.Run(New MainForm())
#Else
            Dim args = Environment.GetCommandLineArgs().Skip(1).Select(Function(a) a.ToLower()).ToHashSet()
            If args.Contains("--service") Then
                ServiceBase.Run(New Service.KantechGatewayService())
            Else
                Application.EnableVisualStyles()
                Application.SetCompatibleTextRenderingDefault(False)
                Application.Run(New MainForm())
            End If
#End If
        End Sub
    End Module
End Namespace