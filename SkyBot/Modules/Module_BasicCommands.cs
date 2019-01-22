// Skybot 2013-2017

using System;
using System.Diagnostics;
using System.IO;

namespace SkyBot.Modules
{
    class Module_BasicCommands : IModule
    {
        private SkyBot bot;

        public Module_BasicCommands(SkyBot b)
        {
            bot = b;

            ID = ModuleList.BasicCommands;
            UsableBy = APIList.All;
        }
        public override string ProcessMessage(ReceivedMessage msg)
        {
            string trigger = msg.API.GetTrigger();

            // only triggers
            if (msg.Text.IndexOf(trigger, 0, 1) >= 0)
            {
                string cmd = msg.Text.Remove(0, 1);

                switch (cmd)
                {
                    case "help":
                        return "Лучше попроси рассказать анекдот";

                    case "status":
                        return Status();

                    case "hardware":
                        return Hardware();

                    case "version":
                        return Version();

                    case "changelog":
                    case "log":
                        return ChangeLog();

                    //default:
                    //    return !cmd.Contains("set")
                    //        ? "Спроси у своей мамы, кто ее кумир"
                    //        : string.Empty;
                }
            }
            return string.Empty;
        }

        private string Status()
        {
            string result = "Состояние API:\n";
            foreach (IConnectionAPI api in bot.APIs)
            {
                result += api.ID.ToString()+ ": " + api.Status.ToString() + "\n";
            }

            result += "\nПодключенные модули:\n";
            foreach (IModule module in bot.Modules)
            {
                result += module.ID.ToString() + "\n";
            }
            result += "\nНе падаем уже с " + bot.startup_time.ToString();
            return result;
        }

        private string Hardware()
        {
            string result = "Жаримся на таких статах:" + 
                "\nBot uptime: " + (DateTime.Now - bot.startup_time) +
                "\nSystem uptime: " + TimeSpan.FromMilliseconds(Environment.TickCount) +
                "\nSystem date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() +
                "\nOS Version: " + Environment.OSVersion.ToString();

            if (Environment.Is64BitOperatingSystem) result += " x64";
            else result += " x86";

            result += "\nEnvironment version: " + Environment.Version.ToString();
            return result;
        }

        private string Version()
        {
            string result = "Я (опять) родился, (блять)!";

            FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(Directory.GetCurrentDirectory() + "\\SkyBot.exe");
            result += "\nVersion info: " + fileVersion.FileVersion;

            FileInfo fileInfo = new FileInfo(Directory.GetCurrentDirectory() + "\\SkyBot.exe");
            result += "\nBuild date: " + fileInfo.LastWriteTime;

            return result;
        }

        private string ChangeLog()
        {
            string result = string.Empty;

            if (File.Exists(Directory.GetCurrentDirectory() + "\\changelog.txt"))
            {
                StreamReader fs = new StreamReader(Directory.GetCurrentDirectory() + "\\changelog.txt", System.Text.Encoding.GetEncoding(1251));
                string log = fs.ReadLine();

                while (log != null)
                {
                    result += log + "\n";
                    log = fs.ReadLine();
                }
                fs.Close();
            }
            else
            {
                result += "В результате террористических действий повстанцев чейнджлог был похищен.";
            }

            return result;
        }
    }
}
