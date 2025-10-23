Imports System.IO
Imports System.Text
Namespace KantechGatewayApp.Infrastructure
    Public Module CsvUtil
        Public Function ReadRows(filePath As String, delimiter As Char, hasHeader As Boolean) As List(Of Dictionary(Of String, String))
            Dim rows As New List(Of Dictionary(Of String, String))()
            Dim headers() As String = Nothing
            Using sr As New StreamReader(filePath, Encoding.UTF8, True)
                Dim line As String = sr.ReadLine()
                If line Is Nothing Then Return rows
                Dim first = SplitCsv(line, delimiter)
                If hasHeader Then
                    headers = first
                Else
                    headers = first.Select(Function(x, i) $"Col{i + 1}").ToArray()
                    rows.Add(ToRow(headers, first))
                End If
                While True
                    line = sr.ReadLine()
                    If line Is Nothing Then Exit While
                    Dim parts = SplitCsv(line, delimiter)
                    rows.Add(ToRow(headers, parts))
                End While
            End Using
            Return rows
        End Function

        Private Function ToRow(headers() As String, parts() As String) As Dictionary(Of String, String)
            Dim d As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            For i = 0 To headers.Length - 1
                Dim v = If(i < parts.Length, parts(i), "")
                d(headers(i).Trim()) = v.Trim()
            Next
            Return d
        End Function

        ' Simple CSV split (no quoted-commas). Replace with a robust parser if needed.
        Private Function SplitCsv(line As String, delimiter As Char) As String()
            Return line.Split(delimiter)
        End Function
    End Module
End Namespace