// Skybot 2013-2016

using System;
using DSharpPlus;
using DSharpPlus.Events;
using DSharpPlus.Objects;

namespace SkyBot.APIs
{
    //
    // Docs: http://dsharpplus.readthedocs.io/en/latest/, https://github.com/NaamloosDT/DiscordSharp_Starter/
    //
    class API_Discord : IConnectionAPI
    {
        public string token;

        public DiscordClient api;

        public API_Discord()
        {
            ID = APIList.Discord;
        }
        public override bool Connect(SkyBot handle)
        {
            Status = APIStatus.Connecting;
            base.Connect(handle);

            token = Config.Read("Discord", "token");

            bool result = false;

            api = new DiscordClient(token, true);
            try
            {
                api.MessageReceived += new EventHandler<DiscordMessageEventArgs>(ReceiveMessages);
                api.SendLoginRequest();
                api.Connect();
                api.Autoconnect = true;
                result = true;
            }
            catch (Exception ex)
            {
                InformationCollector.Error(this, ex.Message);
                return false;
            }

            if (result)
            {
                Status = APIStatus.Connected;
                Parent.Interface.discordStatus.Text = Status.ToString();
                Parent.Interface.discordStatus.ForeColor = System.Drawing.Color.Green;
            }

            return result;
        }
        public override bool Disconnect()
        {
            api.Logout();

            Status = APIStatus.Disabled;
            Parent.Interface.discordStatus.Text = Status.ToString();
            Parent.Interface.discordStatus.ForeColor = System.Drawing.Color.Red;

            return true;
        }
        private void ReceiveMessages(object sender, DiscordMessageEventArgs e)
        {
            Parent.ProcessMessage(e.MessageText, this, e.Channel);
        }
        public override bool SendMessage(string message, object receiver)
        {
            try
            {
                api.SendMessageToChannel(message, (DiscordChannel)receiver, false);
            }
            catch (Exception ex)
            {
                InformationCollector.Error(this, ex.Message);
                return false;
            }
            return true;
        }
    }
}
