// Skybot 2013-2017

namespace SkyBot
{
    public class ReceivedMessage
    {
        public string Text;
        public string Username;
        public object Sender;
        public IConnectionAPI API;

        public object APIMessageClass; // for special cases when we need extended possibilities
    }
}
