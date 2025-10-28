using System;
using System.Windows.Forms;

namespace KantechGatewayApp.KantechGatewayApp
{
    static class Program
    {
        [STAThread()]
        public static void Main()
        {
            /* TODO ERROR: Skipped IfDirectiveTrivia
            #If DEBUG Then
            */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            /* TODO ERROR: Skipped ElseDirectiveTrivia
            #Else
            *//* TODO ERROR: Skipped DisabledTextTrivia
                        Dim args = Environment.GetCommandLineArgs().Skip(1).Select(Function(a) a.ToLower()).ToHashSet()
                        If args.Contains("--service") Then
                            ServiceBase.Run(New Service.KantechGatewayService())
                        Else
                            Application.EnableVisualStyles()
                            Application.SetCompatibleTextRenderingDefault(False)
                            Application.Run(New MainForm())
                        End If
            *//* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
        }
    }
}