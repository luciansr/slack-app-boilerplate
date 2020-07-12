using System.Threading;
using System.Threading.Tasks;
using Models.Api;

namespace Services.EventHandlers
{
    public interface ISlackEventHandler
    {
        Task HandleSlackEventAsync(
            SlackEventBody slackEventBody,
            CancellationToken cancellationToken);
    }
}