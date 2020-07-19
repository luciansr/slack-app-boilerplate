using Models.Events;
using Services.Events.Actions;

namespace Services.Events.Processors
{
    public interface IActionExecutorFactory
    {
        IActionExecutor GetActionExecutor(ActionConfiguration actionConfiguration);
    }

    public class ActionExecutorFactory : IActionExecutorFactory
    {
        public IActionExecutor GetActionExecutor(ActionConfiguration actionConfiguration)
        {
            return actionConfiguration switch
            {
                {Type: ActionType.AnswerToMessage} => new AnswerMessageActionExecutor(),
                _ => new UnknownActionExecutor()
            };
        }
    }
}