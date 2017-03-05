using Bot = Telegram.Bot;
using Dotbot.Models;
using Dotbot.Telegram.Models;

namespace Dotbot.Telegram
{
    public static class TelegramExtensions
    {
        public static TelegramContext ToContext(this Bot.Types.User user, bool isValid)
        {
            return new TelegramContext
            {
                User = user.ToBotUser(),
                Valid = isValid
            };
        }

        public static User GetUser(this Bot.Types.Message message)
        {
            return message.From.ToBotUser();
        }

        public static Room GetRoom(this Bot.Types.Message message)
        {
            return new Room(message.Chat.Id.ToString(), message.Chat.Title);
        }

        public static User ToBotUser(this Bot.Types.User user)
        {
            return new Dotbot.Models.User(user.Id.ToString(), user.Username ?? user.FirstName, $"{user.FirstName} {user.LastName}");
        }
    }
}