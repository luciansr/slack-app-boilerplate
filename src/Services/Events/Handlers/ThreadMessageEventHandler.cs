using Models.Events;
using Services.Events.Handlers.Base;
using Services.Events.Processors;

namespace Services.Events.Handlers
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