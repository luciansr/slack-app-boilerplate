using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Models.Api;
using Models.Events;
using Moq;
using Newtonsoft.Json;
using Services.Events.Handlers;
using Services.Events.Matchers;
using Tests.Models;
using Xunit;

namespace Tests.Services.Events.Matchers
{
    public class CaseSensitiveTextContainsEventMatcherTests
    {
        [Theory]
        [InlineData("<@UKMARFXLH> oi", "oi", true)]
        [InlineData("<@UKMARFXLH> OI", "oi", false)]
        [InlineData("<@UKMARFXLH> OI", null, false)]
        [InlineData(null, "teste", false)]
        [InlineData(null, null, false)]
        public async Task MessageOnChannel_IsRoutedCorrectly(string inputText, string target, bool expected)
        {
            var slackBody = new SlackEventBody
            {
                Event = new SlackEvent
                {
                    Text = inputText
                }
            };

            var eventMatcher = new CaseSensitiveTextContainsEventMatcher(target);

            var result = await eventMatcher.EventMatchesAsync(slackBody);

            Assert.Equal(expected, result.Success);
        }
    }
}