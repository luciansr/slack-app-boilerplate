using Models.Events;
using Services.EventHandlers.Base;
using Services.EventMatchers;

namespace Services.EventHandlers
{
    public class ChannelMessageEventHandler : BaseEventHandler
    {
        public ChannelMessageEventHandler(
            IEventProcessorStorage eventProcessorStorage)
            : base(
                SlackEventType.Message,
                eventProcessorStorage)
        {
        }
    }
}