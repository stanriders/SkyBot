// Skybot 2013-2016

namespace SkyBot
{
    abstract public class IConnectionAPI
    {
        public APIStatus Status;
        public APIList ID;
        public SkyBot Parent;

        public virtual bool Connect(SkyBot handle)
        {
            Parent = handle;
            return true;
        }
        public abstract bool Disconnect();

        public abstract bool SendMessage(string message, long receiver);
    }

    public enum APIList
    {
        Skype,
        VK,
        Telegram,
        Test
    }

    public enum APIStatus
    { 
        Disabled,
        Connecting,
        Connected,
        Broken
    }
}
