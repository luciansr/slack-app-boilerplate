using System.Threading;
using System.Threading.Tasks;
using Models.Api;

namespace Services.EventHandlers
{
    public class ChannelMessageEventHandler : ISlackEventHandler
    {
        public Task HandleSlackEventAsync(SlackEventBody slackEventBody, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}