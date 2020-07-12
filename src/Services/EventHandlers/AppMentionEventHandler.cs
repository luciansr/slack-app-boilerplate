using Models.Events;
using Services.EventHandlers.Base;
using Services.EventMatchers;

namespace Services.EventHandlers
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