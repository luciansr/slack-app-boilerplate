using Models.Events;
using Services.EventHandlers.Base;
using Services.EventMatchers;

namespace Services.EventHandlers
{
    public class UserJoinedEventHandler : BaseEventHandler
    {
        public UserJoinedEventHandler(
            IEventProcessorProvider eventProcessorProvider)
            : base(
                SlackEventType.UserJoined,
                eventProcessorProvider)
        {
        }
    }
}