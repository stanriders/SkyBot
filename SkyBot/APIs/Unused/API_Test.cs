// Skybot 2013-2017

using System.Timers;

namespace SkyBot.APIs
{
    class API_Test : IConnectionAPI
    {
        private static Timer loopTimer = new Timer(3000);
        private int loopIndex = 0;
        public bool loopEnabled
        {
            get { return loopTimer.Enabled; }
            set { loopTimer.Enabled = value; }
        }

        public API_Test()
        {
            ID = APIList.Test;

            loopTimer.Elapsed += new ElapsedEventHandler(TestLoop);
            loopTimer.Enabled = false;
        }
        public override bool Connect(SkyBot handle)
        {
            base.Connect(handle);

            Status = APIStatus.Connected;
            loopEnabled = true;

            return true;
        }

        public override bool Disconnect()
        {
            Status = APIStatus.Disabled;
            loopEnabled = false;

            return true;
        }
        private void TestLoop(object source, ElapsedEventArgs e)
        {
            loopIndex++;

            if ((loopIndex % 2) == 0)

                Parent.ProcessMessage(new ReceivedMessage()
                {
                    API = this,
                    Text = "testmsg n" + loopIndex,
                    Sender = 0,
                    APIMessageClass = 0
                });
            else
                Parent.ProcessMessage(new ReceivedMessage()
                {
                    API = this,
                    Text = "хуй",
                    Sender = 0,
                    APIMessageClass = 0
                });
        }
        public override bool SendMessage(string message, object receiver)
        {
            System.Windows.Forms.MessageBox.Show(message);
            return true;
        }
    }
}
