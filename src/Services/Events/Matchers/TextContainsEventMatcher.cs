using System.Threading.Tasks;
using Models.Api;
using Models.Events;

namespace Services.Events.Matchers
{
    public class TextContainsEventMatcher : IEventMatcher
    {
        public Task<EventMatchResult> EventMatchesAsync(SlackEventBody slackEventBody)
        {
            throw new System.NotImplementedException();
        }
    }
}