// Skybot 2013-2016

using System.Text;
using System.Runtime.InteropServices;

namespace SkyBot
{
    public class Config
    {
        // creating singleton
        private Config() { }
        private static Config instance;
        public static Config Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Config();
                }
                return instance;
            }
        }
        // ini-reading methods
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer, int size, string path);

        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern int WritePrivateString(string section, string key, string str, string path);

        public string Path;

        public string Read(string section, string key)
        {
            StringBuilder buffer = new StringBuilder(1024);

            GetPrivateString(section, key, null, buffer, 1024, Path);

            return buffer.ToString();
        }

        public void Write(string section, string key, string value)
        {
            WritePrivateString(section, key, value, Path);
        }
    }
}
