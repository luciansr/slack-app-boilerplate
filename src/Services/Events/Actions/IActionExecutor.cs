using System.Threading;
using System.Threading.Tasks;
using Models.Api;
using Models.Events;

namespace Services.Events.Actions
{
    public interface IActionExecutor
    {
        Task ExecuteActionAsync(
            SlackEventBody slackEventBody, 
            EventMatchItem[] eventMatchItems, 
            CancellationToken cancellationToken);
    }
}