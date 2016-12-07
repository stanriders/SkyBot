// Skybot 2013-2016

using System.Text;
using System.Runtime.InteropServices;

namespace SkyBot
{
    public static class Config
    {
        // ini-reading methods
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer, int size, string path);

        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern int WritePrivateString(string section, string key, string str, string path);

        public static string Path;

        public static string Read(string section, string key)
        {
            StringBuilder buffer = new StringBuilder(1024);

            GetPrivateString(section, key, null, buffer, 1024, Path);

            return buffer.ToString();
        }

        public static void Write(string section, string key, string value)
        {
            WritePrivateString(section, key, value, Path);
        }
    }
}
