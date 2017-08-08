// Skybot 2013-2017

namespace SkyBot
{
    abstract public class IModule : IBaseModule
    {
        public ModuleList ID;
        public APIList UsableBy = APIList.None;

        abstract public string ProcessMessage(ReceivedMessage msg);

        public string GetTrigger(IConnectionAPI api)
        {
            if (api.ID == APIList.Telegram)
                return "/";
            else
                return "!";
        }
    }

    public enum ModuleList
    {
        Answer,
        Roll,
        BasicCommands,
        Announcer,
        Timer,
        Xyu,
        Test,
        Trigger,
        Random,
        YouTube,
        Boltun,
        Organiser,
        Notepad,
        FicBook
    }
}
