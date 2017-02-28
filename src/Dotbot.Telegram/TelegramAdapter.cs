
namespace Dotbot.Telegram
{
    public class TelegramAdapter : IAdapter
    {
        public string FriendlyName => "Telegram Adapter";

        public IBroker Broker {get;}

        public TelegramAdapter(TelegramBroker broker)
        {    
            Broker = broker;
        }
    }
}
