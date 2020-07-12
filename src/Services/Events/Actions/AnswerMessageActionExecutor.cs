using System.Threading;
using System.Threading.Tasks;
using Models.Api;
using Models.Events;

namespace Services.Events.Actions
{
    public class AnswerMessageActionExecutor : IActionExecutor
    {
        public Task ExecuteActionAsync(SlackEventBody slackEventBody, EventMatchItem[] eventMatchItems, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}