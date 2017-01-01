// Skybot 2013-2017

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
            // only triggers
            if (msg.Text.IndexOf("!", 0, 1) >= 0)
            {
                if (msg.Text == "!help")
                    return "Никто тебе не поможет";

                else if (msg.Text == "!status")
                    return Status();

                else
                    return "Ничо непонял лол";
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
    }
}
