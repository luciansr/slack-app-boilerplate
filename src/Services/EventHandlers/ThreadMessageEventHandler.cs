using Models.Events;
using Services.EventHandlers.Base;
using Services.EventMatchers;

namespace Services.EventHandlers
{
    public class ThreadMessageEventHandler : BaseEventHandler
    {
        public ThreadMessageEventHandler(
            IEventProcessorStorage eventProcessorStorage)
            : base(
                SlackEventType.Message,
                eventProcessorStorage)
        {
        }
    }
}