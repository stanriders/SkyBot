// Skybot 2013-2017

namespace SkyBot
{
    public class ReceivedMessage
    {
        public string Text;
        public object Sender;
        public string Title;
        public IConnectionAPI API;

        public object APIMessageClass; // for special cases when we need extended possibilities
    }
}
