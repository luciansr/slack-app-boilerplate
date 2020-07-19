using Models.Events;
using Services.Events.Handlers.Base;
using Services.Events.Processors;

namespace Services.Events.Handlers
{
    public class ChannelJoinEventHandler : BaseEventHandler
    {
        public ChannelJoinEventHandler(
            IEventProcessorProvider eventProcessorProvider)
            : base(
                SlackEventType.ChannelJoin,
                eventProcessorProvider)
        {
        }
    }
}