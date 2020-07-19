using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Api;

namespace Services.Storage
{
    public class EventStorage
    {
        private readonly ConcurrentDictionary<string, SlackEventBody> _slackConcurrentEvents;

        public EventStorage()
        {
            _slackConcurrentEvents = new ConcurrentDictionary<string, SlackEventBody>();
        }

        public async Task StoreEventAsync(
            SlackEventBody slackEvent, 
            CancellationToken cancellationToken)
        {
            _slackConcurrentEvents[slackEvent.EventId] = slackEvent;
        }

        public async IAsyncEnumerable<SlackEventBody> ConsumeAllEventsAsync(
            CancellationToken cancellationToken)
        {
            foreach (var key in _slackConcurrentEvents.Keys)
            {
                if(_slackConcurrentEvents.TryRemove(key, out var slackEventBody))
                {
                    yield return slackEventBody;
                }
            }
        }
    }
}