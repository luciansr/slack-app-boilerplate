using Models.Events;
using Services.Events.Handlers.Base;
using Services.Events.Processors;

namespace Services.Events.Handlers
{
    public class ReactionAddedEventHandler : BaseEventHandler
    {
        public ReactionAddedEventHandler(
            IEventProcessorProvider eventProcessorProvider)
            : base(
                SlackEventType.ReactionAdded,
                eventProcessorProvider)
        {
        }
    }
}