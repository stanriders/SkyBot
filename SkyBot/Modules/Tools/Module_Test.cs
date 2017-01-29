// Skybot 2013-2017

namespace SkyBot.Modules.Tools
{
    class Module_Test : IModule
    {
        public Configurable configTest1;

        public Module_Test()
        {
            ID = ModuleList.Test;
            UsableBy = APIList.All;

            configTest1 = new Configurable()
            {
                Name = "dick",
                Parent = this
            };
            Configurables.Add(configTest1);

            if (configTest1.Value == string.Empty)
            {
                configTest1.Value = "notbutt!";
            }
        }

        public override string ProcessMessage(ReceivedMessage msg)
        {
            if (msg.Text.IndexOf(GetTrigger(msg.API), 0, 1) >= 0)
                return msg.Text + " is a trigger, processed\n";
            else
                return msg.Text + " processed\n";
        }
    }
}
