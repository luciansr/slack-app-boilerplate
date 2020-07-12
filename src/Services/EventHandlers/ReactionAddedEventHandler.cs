using Models.Events;
using Services.EventHandlers.Base;
using Services.EventMatchers;

namespace Services.EventHandlers
{
    public class ReactionAddedEventHandler : BaseEventHandler
    {
        public ReactionAddedEventHandler(
            IEventProcessorStorage eventProcessorStorage)
            : base(
                SlackEventType.ReactionAdded,
                eventProcessorStorage)
        {
        }
    }
}