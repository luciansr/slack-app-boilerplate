using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models.Api;
using Models.Events;
using Services.EventMatchers;

namespace Services.EventHandlers.Base
{
    public class BaseEventHandler : ISlackEventHandler
    {
        private readonly SlackEventType _slackEventType;
        private readonly IEventProcessorStorage _eventProcessorStorage;

        public BaseEventHandler(
            SlackEventType slackEventType,
            IEventProcessorStorage eventProcessorStorage)
        {
            _slackEventType = slackEventType;
            _eventProcessorStorage = eventProcessorStorage;
        }

        public virtual async Task HandleSlackEventAsync(SlackEventBody slackEventBody, CancellationToken cancellationToken)
        {
            var eventsProcessors = await _eventProcessorStorage.GetEventProcessors(
                slackEventBody.TeamId, 
                slackEventBody.Event.Channel,
                _slackEventType, 
                cancellationToken);

            await Task.WhenAll(
                eventsProcessors.Select(
                    e
                        => e.MatchAndProcessAsync(slackEventBody, cancellationToken)));
        }
    }
}