using Models.Api;

namespace Services.EventMatchers
{
    public interface IEventMatcher
    {
        bool SlackEventMatches(SlackEventBody slackEventBody);
    }
}