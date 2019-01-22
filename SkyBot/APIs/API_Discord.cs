// Skybot 2013-2017

using System;
using DSharpPlus;

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

            api = new DiscordClient(new DiscordConfig()
            {
                AutoReconnect = true,
                Token = token,
            });

            try
            {
                //api.MessageCreated += new AsyncEventHandler<MessageCreateEventArgs>(ReceiveMessages);
                api.ConnectAsync();
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
                Parent.UI.discordStatus.Text = Status.ToString();
                Parent.UI.discordStatus.ForeColor = System.Drawing.Color.Green;
            }

            return result;
        }
        public override bool Disconnect()
        {
            api.Dispose();

            Status = APIStatus.Disabled;
            Parent.UI.discordStatus.Text = Status.ToString();
            Parent.UI.discordStatus.ForeColor = System.Drawing.Color.Red;

            return true;
        }
        private void ReceiveMessages(object sender, MessageCreateEventArgs e)
        {
            Parent.ProcessMessage(new ReceivedMessage()
            {
                API = this,
                Text = e.Message.Content,
                Sender = e.Channel,
                APIMessageClass = e.Message
            });
        }
        public override bool SendMessage(string message, object receiver)
        {
            try
            {
                api.SendMessageAsync( (DiscordChannel)receiver, message, false);
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
