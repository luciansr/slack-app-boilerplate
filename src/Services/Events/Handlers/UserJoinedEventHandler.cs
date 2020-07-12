using Models.Events;
using Services.Events.Handlers.Base;
using Services.Events.Processors;

namespace Services.Events.Handlers
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