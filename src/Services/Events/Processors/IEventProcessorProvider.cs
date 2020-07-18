using System.Collections.Generic;
using System.Threading;
using Models.Events;

namespace Services.Events.Processors
{
    public interface IEventProcessorProvider
    {
        EventProcessor[] GetEventProcessors(
            string teamId,
            string channelId,
            SlackEventType slackEventType,
            CancellationToken cancellationToken);

        void SaveEventProcessingConfiguration(
            SlackProcessingConfiguration slackProcessingConfigurations);
    }
}