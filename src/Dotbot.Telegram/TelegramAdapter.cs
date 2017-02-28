using System;
using System.Threading;
using System.Threading.Tasks;
using Bot = Telegram.Bot;
using Dotbot.Diagnostics;
using Dotbot.Models.Events;
using Dotbot.Models;

namespace Dotbot.Telegram
{
    public class TelegramAdapter : IAdapter, IWorker
    {
        public IBroker Broker => _broker;
        private TelegramBroker _broker;
        private IEventQueue _queue;
        private ILog _log;

        public string FriendlyName => "Telegram Adapter";

        public TelegramAdapter(TelegramBroker broker, IEventQueue queue, ILog log)
        {
            _broker = broker;
            _queue = queue;
            _log = log;
        }

        public async Task<bool> Run(CancellationToken token)
        {
            var client = await _broker.Connect();
            if (!client.Valid) throw new InvalidOperationException("Could not connect to Telegram.");

            _log.Information("Current user is {0}.", client.User.Username);

            _broker.Client.OnMessage += (sender,args) => {
                if (args.Message.Type == Bot.Types.Enums.MessageType.TextMessage) {
                    _queue.Enqueue(new MessageEvent(_broker) {
                        Bot = client.User,
                        Message = new Message { Text = args.Message.Text, User = args.Message.GetUser()},
                        Room = args.Message.GetRoom()
                    });
                }
            };

            _broker.Client.StartReceiving(token);
            return true;
        }
    }
}