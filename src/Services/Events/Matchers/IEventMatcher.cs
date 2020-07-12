using System.Threading.Tasks;
using Models.Api;
using Models.Events;

namespace Services.Events.Matchers
{
    public interface IEventMatcher
    {
        Task<EventMatchResult> EventMatchesAsync(SlackEventBody slackEventBody);
    }
}