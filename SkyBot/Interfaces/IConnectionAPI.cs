// Skybot 2013-2017

namespace SkyBot
{
    abstract public class IConnectionAPI //: IConfigurable
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

        public abstract bool SendMessage(string message, object receiver);
    }

    public enum APIList
    {
        None = 0,
        Skype = 1 << 0,
        VK = 1 << 1,
        Telegram = 1 << 2,
        Discord = 1 << 3,
        Test = 1 << 4,
        All = Skype | VK | Telegram | Discord | Test
    }

    public enum APIStatus
    { 
        Disabled,
        Connecting,
        Connected,
        Broken
    }
}
