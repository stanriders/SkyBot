// Skybot 2013-2017

using System.Collections.Generic;

namespace SkyBot.Modules.Tools
{
    class Module_Xyu : IModule
    {
        private List<string> phraseArray = new List<string>();
        private Configurable phrasePath;

        public Module_Xyu() : base()
        {
            ID = ModuleList.Xyu;
            UsableBy = APIList.All;

            phraseArray = Accessories.ReadFileToArray(phrasePath.Value);
        }

        public override void SetupConfig()
        {
            phrasePath = new Configurable()
            {
                Name = "dbpath",
                ReadableName = "Path to Database",
                Parent = this
            };
            Configurables.Add(phrasePath);
        }

        public string ReturnRandomXyu(string word)
        {
            return phraseArray[RNG.Next(0, phraseArray.Count - 1)].Replace("%w", word);
        }

        public override string ProcessMessage(ReceivedMessage msg)
        {
            string result = string.Empty;
            if (msg.Text.IndexOf("!", 0, 1) >= 0)
            {
                string cmd = msg.Text.Remove(0, 1).ToLower();
                if (cmd.StartsWith("xyu "))
                {
                    result = ReturnRandomXyu(cmd.Remove(0, 4));
                }
            }
            return result;
        }
    }
}
