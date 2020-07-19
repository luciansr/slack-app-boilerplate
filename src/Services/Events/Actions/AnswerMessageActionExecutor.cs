using System.Threading;
using System.Threading.Tasks;
using Models.Api;
using Models.Events;
using Services.Slack;

namespace Services.Events.Actions
{
    public class AnswerMessageActionExecutor : IActionExecutor
    {
        private readonly ISlackClient _slackClient;
        private readonly string _value;

        public AnswerMessageActionExecutor(
            ISlackClient slackClient,
            string value)
        {
            _slackClient = slackClient;
            _value = value;
        }
        
        public async Task ExecuteActionAsync(SlackEventBody slackEventBody, EventMatchItem[] eventMatchItems, CancellationToken cancellationToken)
        {
            await _slackClient.ReplyToMessageAsync(
                slackEventBody.TeamId,
                slackEventBody.Event.Channel,
                slackEventBody.Event.ThreadParentMessageIdentifier ?? slackEventBody.Event.MessageIdentifier,
                _value,
                cancellationToken);
        }
    }
}