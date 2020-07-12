using System.Threading;
using System.Threading.Tasks;
using Models.Events;

namespace Services.EventMatchers
{
    public class EventProcessorStorage : IEventProcessorStorage
    {
        public Task<EventProcessor[]> GetEventProcessors(string teamId, string channelId, SlackEventType slackEventType, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}