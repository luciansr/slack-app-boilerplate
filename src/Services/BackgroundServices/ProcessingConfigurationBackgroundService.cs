using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Services.Events.Processors;
using Services.Storage;

namespace Services.BackgroundServices
{
    public class ProcessingConfigurationBackgroundService : BackgroundService
    {
        private readonly IProcessingConfigurationStorage _configurationStorage;
        private readonly IEventProcessorProvider _processorProvider;

        public ProcessingConfigurationBackgroundService(
            IProcessingConfigurationStorage configurationStorage,
            IEventProcessorProvider processorProvider)
        {
            _configurationStorage = configurationStorage;
            _processorProvider = processorProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var config = await _configurationStorage.GetEventProcessingConfigurationAsync(stoppingToken);
                _processorProvider.SaveEventProcessingConfiguration(config);

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}