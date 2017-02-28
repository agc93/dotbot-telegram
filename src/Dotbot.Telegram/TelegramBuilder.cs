using System;
using Microsoft.Extensions.DependencyInjection;

namespace Dotbot.Telegram
{
    public static class TelegramBuilder
    {
        public static RobotBuilder UseTelegram(this RobotBuilder builder, string token) {
            if (string.IsNullOrWhiteSpace(token)) {
                throw new ArgumentNullException(nameof(token));
            }

            builder.Services.AddSingleton(new TelegramConfiguration(token));

            builder.Services.AddSingleton<TelegramAdapter>();
            builder.Services.AddSingleton<IAdapter>(s => s.GetService<TelegramAdapter>());

            builder.Services.AddSingleton<TelegramWorker>();
            builder.Services.AddSingleton<IWorker>(s => s.GetService<TelegramWorker>());

            builder.Services.AddSingleton<TelegramBroker>();
            builder.Services.AddSingleton<IBroker>(s => s.GetService<TelegramBroker>());

            return builder;
        }
    }
}