using System.Threading;
using System.Threading.Tasks;
using Models.Api;

namespace Services.Events.Handlers
{
    public interface ISlackEventHandler
    {
        Task HandleSlackEventAsync(
            SlackEventBody slackEventBody,
            CancellationToken cancellationToken);
    }
}