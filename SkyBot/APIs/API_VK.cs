// Skybot 2013-2016

using System;
using System.Timers;
using System.Collections.Generic;
using VkNet;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace SkyBot.APIs
{
    //
    // Docs: https://vknet.github.io/vk/
    //
    class API_VK : IConnectionAPI
    {
        private VkApi api;
        private static Timer receiveTimer = new Timer(5000);

        private long appId;
        private string login;
        private string password;

        private long? lastMessage;

        public API_VK()
        {
            ID = APIList.VK;
            Status = APIStatus.Disabled;

            api = new VkApi();

            receiveTimer.Elapsed += new ElapsedEventHandler(ReceiveMessages);
            receiveTimer.Enabled = false;
        }
        private bool Authorize()
        {
            appId = long.Parse(Config.Read("VK", "appID"));
            login = Config.Read("VK", "login");
            password = Config.Read("VK", "password");

            try
            {
                api.Authorize(new ApiAuthParams
                {
                    ApplicationId = (ulong)appId,
                    Login = login,
                    Password = password,
                    Settings = VkNet.Enums.Filters.Settings.Messages
                });
            }
            catch (VkApiException ex)
            {
                ProcessException(ex);
                return false;
            }
            return true;
        }

        public override bool Connect(SkyBot handle)
        {
            base.Connect(handle);

            Status = APIStatus.Connecting;
            Parent.Interface.vkStatus.Text = Status.ToString();
            Parent.Interface.vkStatus.ForeColor = System.Drawing.Color.Yellow;

            bool result = Authorize();

            if (result)
            {
                Status = APIStatus.Connected;
                Parent.Interface.vkStatus.Text = Status.ToString();
                Parent.Interface.vkStatus.ForeColor = System.Drawing.Color.Green;
                receiveTimer.Enabled = true;
            }
            return result;
        }

        public override bool Disconnect()
        {
            receiveTimer.Enabled = false;
            Status = APIStatus.Disabled;
            Parent.Interface.vkStatus.Text = Status.ToString();
            Parent.Interface.vkStatus.ForeColor = System.Drawing.Color.Red;

            api = new VkApi();
            return true;
        }

        private void ReceiveMessages(object source, ElapsedEventArgs e)
        {
            MessagesGetObject result;
            try
            {
                result = api.Messages.Get(new MessagesGetParams
                {
                    Out = 0,
                    TimeOffset = 5,
                    Count = 5
                });
            }
            catch (VkApiException ex)
            {
                ProcessException(ex);
                return;
            }

            ICollection<Message> messages = result.Messages;
            if (messages.Count > 0)
            {
                foreach (Message msg in messages)
                {
                    if (msg.Id != lastMessage)
                    {
                        if (msg.ChatId != null)
                        {
                            Parent.ProcessMessage(msg.Body, this, (long)msg.ChatId);
                        }
                        else
                        {
                            Parent.ProcessMessage(msg.Body, this, (long)msg.UserId);
                        }
                        lastMessage = msg.Id;
                    }
                }
            }
        }

        public override bool SendMessage(string message, object receiver)
        {
            try
            {
                api.Messages.Send(new MessagesSendParams
                {
                    ChatId = (long)receiver,
                    Message = message,
                });
            }
            catch (VkApiException ex)
            {
                ProcessException(ex);
                return false;
            }

            return true;
        }

        private void ProcessException( Exception e )
        {
            if (e.Message.Contains("User authorization failed: access_token has expired"))
            {
                // reconnect
                Disconnect();
                Connect(Parent);
            }
            ExceptionCollector.Error(e.Message);
        }
    }
}
