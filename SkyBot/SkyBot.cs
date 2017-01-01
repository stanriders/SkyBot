// Skybot 2013-2017

using System;
using System.Collections.Generic;

using SkyBot.APIs;
using SkyBot.Modules;
using SkyBot.Modules.Tools;

namespace SkyBot
{
    public class SkyBot
    {
        public MainForm UI;
        public List<IConnectionAPI> APIs = new List<IConnectionAPI>();
        public List<IModule> Modules = new List<IModule>();

        public readonly DateTime startup_time = DateTime.Now;

        public SkyBot(MainForm form)
        {
            UI = form;
            Config.Path = System.Windows.Forms.Application.StartupPath + "\\config.ini";

            // FIXME: make better way of adding modules
            APIs.Add(new API_Test());
            APIs.Add(new API_Skype());
            APIs.Add(new API_Telegram());
            APIs.Add(new API_Discord());
            APIs.Add(new API_VK());
            
            //Modules.Add(new Module_Test());
            Modules.Add(new Module_Answer());
            Modules.Add(new Module_Roll());
            Modules.Add(new Module_Xyu());
            Modules.Add(new Module_Timer(this));
            Modules.Add(new Module_BasicCommands(this));
            //Modules.Add(new Module_Discord_Announcer(APIs.Find(x => (x.ID == APIList.Discord))));
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

        public void ProcessMessage( ReceivedMessage msg )
        {
            string result = string.Empty;

            foreach (IModule module in Modules)
            {
                if ((module.UsableBy & msg.API.ID) == msg.API.ID && result == string.Empty)
                    result = module.ProcessMessage(msg);
            }

            if (result != string.Empty)
                msg.API.SendMessage(result, msg.Sender);
        }
    }
}
