// Skybot 2013-2016

namespace SkyBot
{
    abstract public class IModule
    {
        public ModuleList ID;
        public APIList UsableBy = APIList.None;

        abstract public string ProcessMessage(string msg);
    }

    public enum ModuleList
    {
        Answer,
        Roll,
        BasicCommands,
        Xyu,
        Test
    }
}
