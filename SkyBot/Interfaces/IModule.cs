// Skybot 2013-2016

using System.Collections.Generic;

namespace SkyBot
{
    abstract public class IModule
    {
        public ModuleList ID;
        public APIList UsableBy = APIList.None;

        public List<string> Configurables = new List<string>();

        abstract public string ProcessMessage(string msg);
    }

    public enum ModuleList
    {
        Answer,
        Roll,
        BasicCommands,
        Announcer,
        Xyu,
        Test
    }
}
