using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Models;
using Models.Api;
using Services.EventHandlers;
using Services.Storage;

namespace Services.BackgroundServices
{
    public class EventListenerBackgroundService : BackgroundService
    {
        private readonly EventStorage _eventStorage;
        private readonly SlackEventHandler _slackEventHandler;

        public EventListenerBackgroundService(
            EventStorage eventStorage,
            SlackEventHandler slackEventHandler)
        {
            _eventStorage = eventStorage;
            _slackEventHandler = slackEventHandler;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var semaphoreSlim = new SemaphoreSlim(10, 10);
            while (!stoppingToken.IsCancellationRequested)
            {
                await foreach (var slackEvent in _eventStorage.ConsumeAllEventsAsync(stoppingToken))
                {
                    await semaphoreSlim.WaitAsync(stoppingToken);
                    _ = HandleEventInBackground(slackEvent, semaphoreSlim, stoppingToken);
                }
            }
        }

        private async Task HandleEventInBackground(
            SlackEventBody slackEventBody,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken)
        {
            await _slackEventHandler.HandleSlackEventAsync(slackEventBody, cancellationToken);
            semaphoreSlim.Release();
        }
    }
}