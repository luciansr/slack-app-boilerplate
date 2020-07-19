using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Models.Api;
using Services.Events.Handlers;
using Services.Storage;

namespace Services.BackgroundServices
{
    public class EventListenerBackgroundService : BackgroundService
    {
        private readonly EventStorage _eventStorage;
        private readonly ISlackEventIdentifier _slackEventIdentifier;

        public EventListenerBackgroundService(
            EventStorage eventStorage,
            ISlackEventIdentifier slackEventIdentifier)
        {
            _eventStorage = eventStorage;
            _slackEventIdentifier = slackEventIdentifier;
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

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task HandleEventInBackground(
            SlackEventBody slackEventBody,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken)
        {
            await _slackEventIdentifier.IdentifySlackEventAsync(slackEventBody, cancellationToken);
            semaphoreSlim.Release();
        }
    }
}