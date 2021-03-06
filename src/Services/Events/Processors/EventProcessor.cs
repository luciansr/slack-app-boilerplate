using System.Threading;
using System.Threading.Tasks;
using Models.Api;
using Services.Events.Actions;
using Services.Events.Matchers;

namespace Services.Events.Processors
{
    public class EventProcessor
    {
        private readonly IEventMatcher _eventMatcher;
        private readonly IActionExecutor _actionExecutor;

        public EventProcessor(IEventMatcher eventMatcher, IActionExecutor actionExecutor)
        {
            _eventMatcher = eventMatcher;
            _actionExecutor = actionExecutor;
        }

        public async Task MatchAndProcessAsync(SlackEventBody slackEventBody, CancellationToken cancellationToken)
        {
            var eventMatchResult = await _eventMatcher.EventMatchesAsync(slackEventBody);
            if (eventMatchResult.Success)
            {
                await _actionExecutor.ExecuteActionAsync(
                    slackEventBody, 
                    eventMatchResult.MatchItems, 
                    cancellationToken);
            }
        }
    }
}