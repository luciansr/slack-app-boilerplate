using Models.Events;
using Services.Events.Actions;
using Services.Slack;

namespace Services.Events.Processors
{
    public interface IActionExecutorFactory
    {
        IActionExecutor GetActionExecutor(ActionConfiguration actionConfiguration);
    }

    public class ActionExecutorFactory : IActionExecutorFactory
    {
        private readonly ISlackClient _slackClient;

        public ActionExecutorFactory(ISlackClient slackClient)
        {
            _slackClient = slackClient;
        }
        public IActionExecutor GetActionExecutor(ActionConfiguration actionConfiguration)
        {
            return actionConfiguration switch
            {
                {Type: ActionType.AnswerToMessage} => new AnswerMessageActionExecutor(_slackClient, actionConfiguration.Value),
                _ => new UnknownActionExecutor()
            };
        }
    }
}