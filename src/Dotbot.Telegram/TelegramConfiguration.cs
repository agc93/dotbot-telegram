namespace Dotbot.Telegram
{
    public class TelegramConfiguration
    {
        public TelegramConfiguration(string token)
        {
            Token = token;
        }

        public string Token { get; private set; }
    }
}