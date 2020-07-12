using System.Threading.Tasks;
using Models.Api;
using Models.Events;

namespace Services.Events.Matchers
{
    public class UnknownEventMatcher : IEventMatcher
    {
        public Task<EventMatchResult> EventMatchesAsync(SlackEventBody slackEventBody)
        {
            return Task.FromResult(new EventMatchResult
            {
                Success = false
            });
        }
    }
}