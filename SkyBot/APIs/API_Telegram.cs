// Skybot 2013-2016

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

        public API_Telegram()
        {
            ID = APIList.Telegram;

        }
        public override bool Connect(SkyBot handle)
        {
            base.Connect(handle);
            Status = APIStatus.Connecting;
            Parent.Interface.tgStatus.Text = Status.ToString();
            Parent.Interface.tgStatus.ForeColor = System.Drawing.Color.Yellow;

            token = Config.Read("Telegram", "token");

            try
            {
                // its outside of constructor to make creating child tg bots easier
                api = new TelegramBotClient(token);

                api.OnMessage += ReceiveMessage;
            }
            catch (Exception ex)
            {
                ExceptionCollector.Error(ex.Message);
            }
        
            receiveThread = new Thread( delegate()
            {
                try
                { 
                    api.StartReceiving();
                }
                catch (Exception ex)
                {
                    ExceptionCollector.Error(ex.Message);
                }
            });
            receiveThread.Start();

            bool result = receiveThread.IsAlive;
            if (result)
            {
                Status = APIStatus.Connected;
                Parent.Interface.tgStatus.Text = Status.ToString();
                Parent.Interface.tgStatus.ForeColor = System.Drawing.Color.Green;
            }

            return result;
        }
        public override bool Disconnect()
        {
            receiveThread.Abort();
            api.StopReceiving();

            Status = APIStatus.Disabled;
            Parent.Interface.tgStatus.Text = Status.ToString();
            Parent.Interface.tgStatus.ForeColor = System.Drawing.Color.Red;

            return true;
        }
        public virtual void ReceiveMessage(object sender, MessageEventArgs messageEventArgs)
        {
            Message msg = messageEventArgs.Message;

            if (msg == null || msg.Type != MessageType.TextMessage) return;

            Parent.ProcessMessage(msg.Text, this, msg.Chat.Id);
        }
        public override bool SendMessage(string message, object receiver)
        {
            api.SendTextMessageAsync((long)receiver, message);
            return true;
        }
    }
}
