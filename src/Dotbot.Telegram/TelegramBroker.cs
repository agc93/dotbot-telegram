using System;
using System.Threading.Tasks;
using Dotbot.Models;
using Dotbot.Telegram.Models;
using Telegram.Bot;

namespace Dotbot.Telegram
{
    public class TelegramBroker : Dotbot.IBroker
    {
        private readonly TelegramConfiguration _configuration;
        internal TelegramBotClient Client { get; private set; }

        public TelegramBroker(TelegramConfiguration configuration)
        {
            if (configuration?.Token == null) throw new ArgumentNullException(nameof(configuration));
            _configuration = configuration;
        }

        internal async Task<TelegramContext> Connect()
        {
            var bot = new TelegramBotClient(_configuration.Token);
            var valid = await bot.TestApiAsync();
            var me = await bot.GetMeAsync();
            Client = bot;
            return me.ToContext(valid && me != null);
        }

        public async Task Broadcast(Room room, string text)
        {
            var chatId = await Client.GetChatAsync(room.Id);
            var message = await Client.SendTextMessageAsync(chatId.Id, text);
        }

        public async Task Reply(Room room, User fromUser, string text)
        {
            var chat = await Client.GetChatAsync(room.Id);
            var message = await Client.SendTextMessageAsync(chat.Id, $"{fromUser.Username}: {text}");
        }
    }
}