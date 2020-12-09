using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Snippy.Infrastructure
{
    internal static class Extensions
    {
        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);

        public static Process Run(this string filePath)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer",
                    Arguments = "\"" + filePath + "\"",
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                });
            }

            return Process.Start(new ProcessStartInfo(filePath)
            {
                UseShellExecute = true
            });
        }

        public static Process RunWithCode(this string filePath)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "code",
                Arguments = "\"" + filePath + "\"",
                UseShellExecute = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            });
        }
    }
}
