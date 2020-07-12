using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models.Api;
using Models.Events;
using Services.Events.Processors;

namespace Services.Events.Handlers.Base
{
    public class BaseEventHandler : ISlackEventHandler
    {
        private readonly SlackEventType _slackEventType;
        private readonly IEventProcessorProvider _eventProcessorProvider;

        protected BaseEventHandler(
            SlackEventType slackEventType,
            IEventProcessorProvider eventProcessorProvider)
        {
            _slackEventType = slackEventType;
            _eventProcessorProvider = eventProcessorProvider;
        }

        public virtual async Task HandleSlackEventAsync(SlackEventBody slackEventBody, CancellationToken cancellationToken)
        {
            var eventsProcessors = _eventProcessorProvider.GetEventProcessors(
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