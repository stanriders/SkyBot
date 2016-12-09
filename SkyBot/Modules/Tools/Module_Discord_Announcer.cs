using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;
using DSharpPlus;
using SkyBot.APIs;
using DSharpPlus.Objects;

namespace SkyBot.Modules
{
    class Module_Discord_Announcer : IModule
    {
        private DiscordClient api;

        public Module_Discord_Announcer(API_Discord client)
        {
            UsableBy = APIList.Discord;
            ID = ModuleList.Announcer;

            api = client.api;
            api.VoiceStateUpdate += VoiceStateUpdate;
        }

        private void VoiceStateUpdate(object sender, DiscordVoiceStateUpdateEventArgs e)
        {
        }

        public override string ProcessMessage(string msg)
        {
            return string.Empty;
        }
    }
}
