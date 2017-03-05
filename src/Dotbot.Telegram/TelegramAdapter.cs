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
            try
            {
                var client = await _broker.Connect();
                if (!client.Valid) throw new InvalidOperationException("Could not connect to Telegram.");

                _log.Information("Current user is {0}.", client.User.Username);

                _broker.Client.OnMessage += (sender, args) =>
                {
                    if (args.Message.Type == Bot.Types.Enums.MessageType.TextMessage)
                    {
                        _queue.Enqueue(new MessageEvent(
                            client.User,
                            args.Message.GetRoom(),
                            new Message(args.Message.GetUser(), args.Message.Text),
                            _broker));
                    }
                };

                _broker.Client.StartReceiving(token);
                _log.Information("Adapter started, now listening.");
                token.WaitHandle.WaitOne(Timeout.Infinite);
                _log.Information("Adapter terminating.");
                _broker.Client.StopReceiving();
                return true;
            }
            finally
            {
                _broker.Client.StopReceiving();
            }
        }
    }
}