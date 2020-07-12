using Models.Events;
using Services.Events.Handlers.Base;
using Services.Events.Processors;

namespace Services.Events.Handlers
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