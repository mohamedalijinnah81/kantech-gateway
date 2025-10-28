using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KantechGatewayApp.KantechGatewayApp.Infrastructure
{
    public static class CsvUtil
    {
        public static List<Dictionary<string, string>> ReadRows(string filePath, char delimiter, bool hasHeader)
        {
            var rows = new List<Dictionary<string, string>>();
            string[] headers = null;
            using (var sr = new StreamReader(filePath, Encoding.UTF8, true))
            {
                string line = sr.ReadLine();
                if (line is null)
                    return rows;
                string[] first = SplitCsv(line, delimiter);
                if (hasHeader)
                {
                    headers = first;
                }
                else
                {
                    headers = first.Select((x, i) => $"Col{i + 1}").ToArray();
                    rows.Add(ToRow(headers, first));
                }
                while (true)
                {
                    line = sr.ReadLine();
                    if (line is null)
                        break;
                    string[] parts = SplitCsv(line, delimiter);
                    rows.Add(ToRow(headers, parts));
                }
            }
            return rows;
        }

        private static Dictionary<string, string> ToRow(string[] headers, string[] parts)
        {
            var d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0, loopTo = headers.Length - 1; i <= loopTo; i++)
            {
                string v = i < parts.Length ? parts[i] : "";
                d[headers[i].Trim()] = v.Trim();
            }
            return d;
        }

        // Simple CSV split (no quoted-commas). Replace with a robust parser if needed.
        private static string[] SplitCsv(string line, char delimiter)
        {
            return line.Split(delimiter);
        }
    }
}