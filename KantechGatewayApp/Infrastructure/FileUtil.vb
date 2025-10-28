Imports System.IO
Namespace KantechGatewayApp.Infrastructure
    Public Module FileUtil
        Public Function EnsureDir(path As String) As String
            Directory.CreateDirectory(path) : Return path
        End Function

        Public Function Combine(root As String, subPath As String) As String
            If Path.IsPathRooted(subPath) Then Return subPath
            Return Path.Combine(root, subPath)
        End Function

        Public Sub SafeMove(src As String, destDir As String)
            EnsureDir(destDir)
            Dim dest = Path.Combine(destDir, Path.GetFileName(src))
            If File.Exists(dest) Then
                Dim stamped = Path.Combine(destDir, $"{Path.GetFileNameWithoutExtension(src)}_{DateTime.Now:ddMMyyyy_HHmmssfff}{Path.GetExtension(src)}")
                File.Move(src, stamped)
            Else
                File.Move(src, dest)
            End If
        End Sub
    End Module
End Namespace