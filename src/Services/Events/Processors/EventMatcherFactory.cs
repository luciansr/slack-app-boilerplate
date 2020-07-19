using Models.Events;
using Services.Events.Matchers;

namespace Services.Events.Processors
{
    public interface IEventMatcherFactory
    {
        IEventMatcher GetEventMatcher(EventMatchConfiguration matchConfiguration);
    }

    public class EventMatcherFactory : IEventMatcherFactory
    {
        public IEventMatcher GetEventMatcher(EventMatchConfiguration matchConfiguration)
        {
            return matchConfiguration switch
            {
                {Type: MatchType.TextContains} => new TextContainsEventMatcher(matchConfiguration.Target),
                _ => new UnknownEventMatcher()
            };
        }
    }
}