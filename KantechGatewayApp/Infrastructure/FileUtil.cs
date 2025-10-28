using System;
using System.IO;

namespace KantechGatewayApp.KantechGatewayApp.Infrastructure
{
    public static class FileUtil
    {
        public static string EnsureDir(string path)
        {
            Directory.CreateDirectory(path);
            return path;
        }

        public static string Combine(string root, string subPath)
        {
            if (Path.IsPathRooted(subPath))
                return subPath;
            return Path.Combine(root, subPath);
        }

        public static void SafeMove(string src, string destDir, string jobKey)
        {
            EnsureDir(destDir);
            string dest = Path.Combine(destDir, Path.GetFileName(src));
            string stamped = Path.Combine(destDir, $"{jobKey}_{Path.GetFileNameWithoutExtension(src)}_{DateTime.Now:ddMMyyyy_HHmmssfff}{Path.GetExtension(src)}");
            File.Move(src, stamped);
        }
    }
}