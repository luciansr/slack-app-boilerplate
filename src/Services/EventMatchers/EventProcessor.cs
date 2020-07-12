using System.Threading;
using System.Threading.Tasks;
using Models.Api;

namespace Services.EventMatchers
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
            if (_eventMatcher.SlackEventMatches(slackEventBody))
            {
                await _actionExecutor.ExecuteActionAsync(slackEventBody, cancellationToken);
            }
        }
    }
}