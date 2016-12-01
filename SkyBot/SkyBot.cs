// Skybot 2013-2016

using System.Collections.Generic;

using SkyBot.APIs;
using SkyBot.Modules;
using SkyBot.Modules.Tools;

namespace SkyBot
{
    public class SkyBot
    {
        public MainForm Interface;
        public List<IConnectionAPI> APIs = new List<IConnectionAPI>();
        public List<IModule> Modules = new List<IModule>();

        public SkyBot(MainForm form)
        {
            Interface = form;
            Config.Instance.Path = System.Windows.Forms.Application.StartupPath + "\\config.ini";

            // FIXME: make better way of adding modules
            APIs.Add(new API_Test());
            APIs.Add(new API_Skype());
            APIs.Add(new API_Telegram());
            APIs.Add(new API_VK());
            
            //Modules.Add(new Module_Test());
            Modules.Add(new Module_Answer());
            Modules.Add(new Module_Roll());
        }

        public void EnableAPI(APIList id)
        {
            IConnectionAPI api = APIs.Find(x => (x.ID == id && x.Status == APIStatus.Disabled));
            api?.Connect(this);
        }

        public void DisableAPI(APIList id)
        {
            IConnectionAPI api = APIs.Find(x => (x.ID == id && x.Status != APIStatus.Disabled));
            api?.Disconnect();
        }

        public void ProcessMessage( string msg, IConnectionAPI api, long source )
        {
            string result = string.Empty;

            foreach (IModule module in Modules)
            {
                if ((module.UsableBy & api.ID) == api.ID)
                    result += module.ProcessMessage(msg);
            }

            if (result != string.Empty)
                api.SendMessage(result, source);
        }
    }
}
