using Models.Events;
using Services.EventHandlers.Base;
using Services.EventMatchers;

namespace Services.EventHandlers
{
    public class UserJoinedEventHandler : BaseEventHandler
    {
        public UserJoinedEventHandler(
            IEventProcessorStorage eventProcessorStorage)
            : base(
                SlackEventType.UserJoined,
                eventProcessorStorage)
        {
        }
    }
}