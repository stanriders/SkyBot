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
            appId = long.Parse(Config.Instance.Read("VK", "appID"));
            login = Config.Instance.Read("VK", "login");
            password = Config.Instance.Read("VK", "password");

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
            catch (VkApiAuthorizationException ex)
            {
                System.Windows.Forms.MessageBox.Show("Incorrect auth info! " + ex.Message);
                return false;
            }
            return true;
        }

        public override bool Connect(SkyBot handle)
        {
            Status = APIStatus.Connecting;
            base.Connect(handle);
            
            bool result = Authorize();

            if (result)
            {
                Status = APIStatus.Connected;
                receiveTimer.Enabled = true;
            }
            return result;
        }

        public override bool Disconnect()
        {
            receiveTimer.Enabled = false;
            Status = APIStatus.Disabled;
            throw new NotImplementedException();
        }

        private void ReceiveMessages(object source, ElapsedEventArgs e)
        {
            MessagesGetObject result = api.Messages.Get(new MessagesGetParams
            {
                Out = 0,
                TimeOffset = 5,
                Count = 5
            });

            ICollection<Message> messages = result.Messages;
            if (messages.Count > 0)
            {
                foreach (Message msg in messages)
                {
                    if (msg.ChatId != null)
                    {
                        Parent.ProcessMessage(msg.Body, this, (long)msg.ChatId);
                    }
                    else
                    {
                        Parent.ProcessMessage(msg.Body, this, (long)msg.UserId);
                    }
                }
            }
        }

        public override bool SendMessage(string message, long receiver)
        {
            try
            {
                api.Messages.Send(new MessagesSendParams
                {
                    ChatId = receiver,
                    Message = message,
                });
            }
            catch (VkApiException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }
    }
}
