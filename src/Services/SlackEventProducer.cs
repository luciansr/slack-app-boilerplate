using System.Threading;
using System.Threading.Tasks;
using Models;
using Models.Api;
using Services.Storage;

namespace Services
{
    public class SlackEventProducer
    {
        private readonly EventStorage _eventStorage;

        public SlackEventProducer(EventStorage eventStorage)
        {
            _eventStorage = eventStorage;
        }
        
        public Task ReceiveSlackEvent(
            SlackEventBody slackEvent, 
            CancellationToken cancellationToken)
        {
            return _eventStorage.StoreEventAsync(slackEvent, cancellationToken);
        }
    }
}