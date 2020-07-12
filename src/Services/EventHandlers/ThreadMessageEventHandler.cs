using Models.Events;
using Services.EventHandlers.Base;
using Services.EventMatchers;

namespace Services.EventHandlers
{
    public class ThreadMessageEventHandler : BaseEventHandler
    {
        public ThreadMessageEventHandler(
            IEventProcessorProvider eventProcessorProvider)
            : base(
                SlackEventType.Message,
                eventProcessorProvider)
        {
        }
    }
}