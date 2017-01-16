// Skybot 2013-2017

using System;
using System.Collections.Generic;
using System.IO;

namespace SkyBot
{
    public static class Accessories
    {
        public static List<string> ReadFileToArray(string path, bool encoding = false)
        {
            List<string> result = new List<string>();
            try
            {
                StreamReader fs = new StreamReader(path, (!encoding) ? System.Text.Encoding.GetEncoding(1251) : System.Text.Encoding.GetEncoding(65001));
                string line = fs.ReadLine();

                while (line != null)
                {
                    result.Add(line);
                    line = fs.ReadLine();
                }

                fs.Close();
            }
            catch (Exception e) { InformationCollector.Error("ReadFileToArray", e.Message); }
            return result;
        }

        public static string ReadFileToString(string path, bool encoding = false)
        {
            string result = string.Empty;
            try
            {
                StreamReader fs = new StreamReader(path, (!encoding) ? System.Text.Encoding.GetEncoding(1251) : System.Text.Encoding.GetEncoding(65001));
                result = fs.ReadToEnd();
                fs.Close();
            }
            catch (Exception e) { InformationCollector.Error("ReadFileToArray", e.Message); }
            return result;
        }
    }
}
