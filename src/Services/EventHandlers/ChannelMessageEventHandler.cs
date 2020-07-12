using Models.Events;
using Services.EventHandlers.Base;
using Services.EventMatchers;

namespace Services.EventHandlers
{
    public class ChannelMessageEventHandler : BaseEventHandler
    {
        public ChannelMessageEventHandler(
            IEventProcessorProvider eventProcessorProvider)
            : base(
                SlackEventType.Message,
                eventProcessorProvider)
        {
        }
    }
}