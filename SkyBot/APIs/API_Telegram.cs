// Skybot 2013-2017

using System;
using System.Threading;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SkyBot.APIs
{
    //
    // Docs: https://github.com/MrRoundRobin/telegram.bot
    // 

    class API_Telegram : IConnectionAPI
    {
        public static string token = string.Empty;
        public static TelegramBotClient api;

        private Thread receiveThread;

        public override string GetTrigger() { return "/"; }

        public API_Telegram()
        {
            ID = APIList.Telegram;
        }
        
        public override bool Connect(SkyBot handle)
        {
            base.Connect(handle);
            Status = APIStatus.Connecting;
            Parent.UI.tgStatus.Text = Status.ToString();
            Parent.UI.tgStatus.ForeColor = System.Drawing.Color.Yellow;

            token = Config.Read("Telegram", "token");

            try
            {
                // its outside of constructor to make creating child tg bots easier
                api = new TelegramBotClient(token);

                api.OnMessage += ReceiveMessage;
                api.OnReceiveError += OnError;
                api.OnReceiveGeneralError += OnError;
            }
            catch (Exception ex)
            {
                InformationCollector.Error(this, ex.Message);
            }
        
            receiveThread = new Thread( delegate()
            {
                try
                {
                    api.TestApiAsync();
                    api.StartReceiving();

                    if(api.IsReceiving)
                        InformationCollector.Info(this, "Receiving...");
                    else
                        InformationCollector.Error(this, "NOT receiving...");
                }
                catch (Exception ex)
                {
                    InformationCollector.Error(this, ex.Message);
                }
            });
            receiveThread.Start();

            bool result = receiveThread.IsAlive;
            if (result)
            {
                Status = APIStatus.Connected;
                Parent.UI.tgStatus.Text = Status.ToString();
                Parent.UI.tgStatus.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                InformationCollector.Error(this, "receiveThread is NOT alive!");
                Disconnect();
            }

            return result;
        }
        public override bool Disconnect()
        {
            api.StopReceiving();
            receiveThread.Abort();

            Status = APIStatus.Disabled;
            Parent.UI.tgStatus.Text = Status.ToString();
            Parent.UI.tgStatus.ForeColor = System.Drawing.Color.Red;

            return true;
        }
        public virtual void ReceiveMessage(object sender, MessageEventArgs messageEventArgs)
        {
            Message msg = messageEventArgs.Message;

            if (msg == null || msg.Type != MessageType.TextMessage)
                return;

            Parent.ProcessMessage(new ReceivedMessage()
            {
                API = this,
                Text = msg.Text,
                Sender = msg.Chat.Id,
                APIMessageClass = msg
            });
        }
        public override bool SendMessage(string message, object receiver)
        {
            api.SendTextMessageAsync((long)receiver, message);
            return true;
        }

        private void OnError(object sender, ReceiveErrorEventArgs receiveEventArgs)
        {
            InformationCollector.Error(this, receiveEventArgs.ApiRequestException.Message);
        }
        private void OnError(object sender, ReceiveGeneralErrorEventArgs receiveEventArgs)
        {
            InformationCollector.Error(this, receiveEventArgs.Exception.Message);
        }
    }
}
