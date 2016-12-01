// Skybot 2013-2016

namespace SkyBot
{
    abstract public class IModule
    {
        public ModuleList ID;

        abstract public string ProcessMessage(string msg);
    }

    public enum ModuleList
    {
        Answer,
        Roll,
        Youtube,
        Xyu,
        Test
    }
}
