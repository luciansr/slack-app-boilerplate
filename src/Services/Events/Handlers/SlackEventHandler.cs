using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models.Api;
using Models.Events;
using Services.Events.Processors;

namespace Services.Events.Handlers
{
    public interface ISlackEventHandler
    {
        Task HandleSlackEventAsync(
            SlackEventType slackEventType,
            SlackEventBody slackEventBody,
            CancellationToken cancellationToken);
    }
    
    public class SlackEventHandler : ISlackEventHandler
    {
        private readonly IEventProcessorProvider _eventProcessorProvider;

        public SlackEventHandler(
            IEventProcessorProvider eventProcessorProvider)
        {
            _eventProcessorProvider = eventProcessorProvider;
        }

        public async Task HandleSlackEventAsync(
            SlackEventType slackEventType, SlackEventBody slackEventBody, CancellationToken cancellationToken)
        {
            var eventsProcessors = _eventProcessorProvider.GetEventProcessors(
                slackEventBody.TeamId, 
                slackEventBody.Event.Channel,
                slackEventType);

            await Task.WhenAll(
                eventsProcessors.Select(
                    e
                        => e.MatchAndProcessAsync(slackEventBody, cancellationToken)));
        }
    }
}