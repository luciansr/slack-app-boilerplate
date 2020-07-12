using System.Threading;
using System.Threading.Tasks;
using Models.Api;

namespace Services.EventHandlers
{
    public class UserJoinedEventHandler : ISlackEventHandler
    {
        public Task HandleSlackEventAsync(SlackEventBody slackEventBody, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}