// Skybot 2013-2016

using System;
//using SKYPE4COMLib;

namespace SkyBot.APIs
{
    public class API_Skype : IConnectionAPI
    {
        //Skype skype;

        public API_Skype()
        {
            ID = APIList.Skype;
            Status = APIStatus.Disabled;

            //skype = new Skype();
        }
    
        public override bool Connect(SkyBot handle)
        {
            base.Connect(handle);
            //skype.Attach(7, false);
            //skype.MessageStatus += new _ISkypeEvents_MessageStatusEventHandler(RecieveMessage);

            //Status = APIStatus.Connecting;
            //return true;
            throw new NotImplementedException();
        }

        public override bool Disconnect()
        {
            //skype.Attach(7, false);
            //skype.MessageStatus += new _ISkypeEvents_MessageStatusEventHandler(RecieveMessage);

            //Status = APIStatus.Disabled;
            //return true;
            throw new NotImplementedException();
        }

        public void RecieveMessage(/*ChatMessage message, TChatMessageStatus status*/)
        {
            throw new NotImplementedException();
        }

        public override bool SendMessage(string message, object receiver)
        {
            throw new NotImplementedException();
        }
    }
}
