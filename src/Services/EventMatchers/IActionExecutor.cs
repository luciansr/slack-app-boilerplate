using System.Threading;
using System.Threading.Tasks;
using Models.Api;

namespace Services.EventMatchers
{
    public interface IActionExecutor
    {
        Task ExecuteActionAsync(SlackEventBody slackEventBody, CancellationToken cancellationToken);
    }
}