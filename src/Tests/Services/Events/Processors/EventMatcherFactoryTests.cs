using System;
using Models.Events;
using Services.Events.Matchers;
using Services.Events.Processors;
using Xunit;

namespace Tests.Services.Events.Processors
{
    public class EventMatcherFactoryTests
    {
        [Theory]
        [InlineData(MatchType.TextContains, typeof(TextContainsEventMatcher))]
        [InlineData(MatchType.CaseSensitiveTextContains, typeof(CaseSensitiveTextContainsEventMatcher))]
        public void ProviderCreatesCorrectMatchers(MatchType matchConfiguration, Type eventMatcherType)
        {
            var eventMatcherFactory = new EventMatcherFactory();

            var eventMatcher = eventMatcherFactory.GetEventMatcher(
                new EventMatchConfiguration
                {
                    Type = matchConfiguration
                });
            
            Assert.Equal(eventMatcherType, eventMatcher.GetType());
        }
    }
}