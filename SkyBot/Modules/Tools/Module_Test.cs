// Skybot 2013-2016

namespace SkyBot.Modules.Tools
{
    class Module_Test : IModule
    {
        public string configTest1;

        public Module_Test()
        {
            ID = ModuleList.Test;
            UsableBy = APIList.All;

            configTest1 = Config.Read("Main", "dick");

            if (configTest1 == string.Empty)
            {
                configTest1 = "notbutt!";
                Config.Write("Main", "dick", configTest1);
            }
        }

        public override string ProcessMessage(string msg)
        {
            //System.Windows.Forms.MessageBox.Show(configTest1);
            Config.Write("Main", "dick", configTest1);

            if (msg.IndexOf("!", 0, 1) >= 0)
                return msg + " is a trigger, processed\n";
            else
                return msg + " processed\n";
        }
    }
}
