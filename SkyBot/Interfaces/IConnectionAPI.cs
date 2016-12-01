// Skybot 2013-2016

namespace SkyBot
{
    abstract public class IConnectionAPI
    {
        public APIStatus Status = APIStatus.Disabled;
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
        None = 0x0,
        Skype = 0x1,
        VK = 0x2,
        Telegram = 0x4,
        Test = 0x8,
        All = Skype | VK | Telegram | Test
    }

    public enum APIStatus
    { 
        Disabled,
        Connecting,
        Connected,
        Broken
    }
}
