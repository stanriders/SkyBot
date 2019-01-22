// Skybot 2013-2017

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

        public static string Read(string section, string key, string path = "")
        {
            StringBuilder buffer = new StringBuilder(1024);

            GetPrivateString(section, key, null, buffer, 1024, path == "" ? Path : path);

            return buffer.ToString();
        }

        public static void Write(string section, string key, string value, string path = "")
        {
            WritePrivateString(section, key, value, path == "" ? Path : path);
        }
    }

    public class Configurable
    {
        public IModule Parent;
        public string Name;

        private string pReadable = string.Empty;
        public string ReadableName
        {
            get
            {
                if (pReadable == string.Empty)
                    return Name;
                else
                    return pReadable;
            }
            set
            {
                pReadable = value;
            }
               
        }

        private bool needsUpdate = true;
        private string pValue;
        public string Value
        {
            get
            {
                if (needsUpdate)
                {
                    pValue = Config.Read(Parent.ID.ToString(), Name);
                    if (pValue == "")
                        return pDefValue;

                    needsUpdate = false;
                }
                return pValue;
            }
            set
            {
                pValue = value;
                Config.Write(Parent.ID.ToString(), Name, value);
            }
        }

        private string pDefValue;
        public string DefaultValue
        {
            get
            {
                return pDefValue;
            }
            set
            {
                pValue = pDefValue = value;
            }
        }
    }
}
