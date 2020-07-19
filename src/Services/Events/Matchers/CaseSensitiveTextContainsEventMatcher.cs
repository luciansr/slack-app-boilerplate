using System;
using System.Threading.Tasks;
using Models.Api;
using Models.Events;

namespace Services.Events.Matchers
{
    public class CaseSensitiveTextContainsEventMatcher : IEventMatcher
    {
        private readonly string _target;

        public CaseSensitiveTextContainsEventMatcher(string target)
        {
            _target = target;
        }
        
        public Task<EventMatchResult> EventMatchesAsync(SlackEventBody slackEventBody)
        {
            var result = new EventMatchResult
            {
                Success = _target != null &&
                          (slackEventBody?.Event?.Text?.Contains(_target, StringComparison.Ordinal) ?? false)
            };

            if (result.Success)
            {
                result.MatchItems = new[]
                {
                    new EventMatchItem(EventMatchItemType.Text, slackEventBody?.Event?.Text)
                };
            }
            
            return Task.FromResult(result);
        }
    }
}