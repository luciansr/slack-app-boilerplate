using System.Threading;
using System.Threading.Tasks;
using Models.Events;

namespace Services.EventMatchers
{
    public interface IEventProcessorStorage
    {
        Task<EventProcessor[]> GetEventProcessors(
            string teamId,
            string channelId,
            SlackEventType slackEventType,
            CancellationToken cancellationToken);
    }
}