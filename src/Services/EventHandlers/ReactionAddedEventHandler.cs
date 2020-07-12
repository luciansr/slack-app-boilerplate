using System.Threading;
using System.Threading.Tasks;
using Models.Api;

namespace Services.EventHandlers
{
    public class ReactionAddedEventHandler : ISlackEventHandler
    {
        public Task HandleSlackEventAsync(SlackEventBody slackEventBody, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}