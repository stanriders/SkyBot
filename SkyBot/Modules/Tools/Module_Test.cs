// Skybot 2013-2016

namespace SkyBot.Modules.Tools
{
    class Module_Test : IModule
    {
        public string configTest1;
        public string configTest2;

        public Module_Test()
        {
            ID = ModuleList.Test;
            UsableBy = APIList.All;

            Configurables.Add("dick");
            configTest1 = Config.Read(ID.ToString(), "dick");

            if (configTest1 == string.Empty)
            {
                configTest1 = "notbutt!";
                Config.Write(ID.ToString(), "dick", configTest1);
            }

            Configurables.Add("benis");
            configTest2 = Config.Read(ID.ToString(), "benis");

            if (configTest2 == string.Empty)
            {
                configTest2 = "notbenis!";
                Config.Write(ID.ToString(), "benis", configTest2);
            }
        }

        public override string ProcessMessage(string msg)
        {
            //System.Windows.Forms.MessageBox.Show(configTest1);
            Config.Write(ID.ToString(), "dick", configTest1);

            if (msg.IndexOf("!", 0, 1) >= 0)
                return msg + " is a trigger, processed\n";
            else
                return msg + " processed\n";
        }
    }
}
