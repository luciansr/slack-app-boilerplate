using Models.Events;
using Services.Events.Handlers.Base;
using Services.Events.Processors;

namespace Services.Events.Handlers
{
    public class AppMentionEventHandler : BaseEventHandler
    {
        public AppMentionEventHandler(
            IEventProcessorProvider eventProcessorProvider)
            : base(
                SlackEventType.AppMention,
                eventProcessorProvider)
        {
        }
    }
}