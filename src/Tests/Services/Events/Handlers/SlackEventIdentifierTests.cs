using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Models.Events;
using Moq;
using Newtonsoft.Json;
using Services.Events.Handlers;
using Tests.Models;
using Xunit;

namespace Tests.Services.Events.Handlers
{
    public class SlackEventIdentifierTests
    {
        [Theory]
        [InlineData("./SlackInputs/AppMention.json", SlackEventType.AppMention)]
        [InlineData("./SlackInputs/MessageOnPrivateChannel.json", SlackEventType.Message)]
        [InlineData("./SlackInputs/MessageOnPublicChannel.json", SlackEventType.Message)]
        [InlineData("./SlackInputs/MessageWithLinkInsideThread.json", SlackEventType.ThreadMessage)]
        [InlineData("./SlackInputs/ReactionAddedOnChannelMessage.json", SlackEventType.ReactionAdded)]
        [InlineData("./SlackInputs/ThreadMessage.json", SlackEventType.ThreadMessage)]
        [InlineData("./SlackInputs/UserJoinedChannel.json", SlackEventType.ChannelJoin)]
        public async Task MessageOnChannel_IsRoutedCorrectly(string inputEvent, SlackEventType expectedEvent)
        {
            var slackBody = GetSlackEventBody(inputEvent);

            var slackRouterMock = new Mock<ISlackEventHandler>();
            
            var slackEventIdentifier = new SlackEventIdentifier(slackRouterMock.Object);

            await slackEventIdentifier.IdentifySlackEventAsync(slackBody.Body, CancellationToken.None);

            slackRouterMock.Verify(x => x.HandleSlackEventAsync(It.Is<SlackEventType>(
                type =>  type == expectedEvent), slackBody.Body, CancellationToken.None), Times.Once);
        }


        private SlackMockBody GetSlackEventBody(string filename)
        {
            var allLines = string.Join(Environment.NewLine, File.ReadAllLines(filename));
            
            return JsonConvert.DeserializeObject<SlackMockBody>(allLines);
        }
    }
}