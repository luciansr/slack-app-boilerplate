using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Models;
using Models.Api;

namespace Services
{
    public class EventStorage
    {
        private readonly Channel<SlackEventBody> _slackEvents;

        public EventStorage()
        {
            _slackEvents = Channel.CreateUnbounded<SlackEventBody>();
        }

        public async Task StoreEventAsync(
            SlackEventBody slackEvent, 
            CancellationToken cancellationToken)
        {
            await _slackEvents.Writer.WriteAsync(slackEvent, cancellationToken);
        }

        public IAsyncEnumerable<SlackEventBody> ConsumeAllEventsAsync(
            CancellationToken cancellationToken)
        {
            return _slackEvents.Reader.ReadAllAsync(cancellationToken);
        }
    }
}