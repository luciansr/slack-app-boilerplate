using System.Collections.Generic;
using System.Threading;
using Models.Events;

namespace Services.EventMatchers
{
    public interface IEventProcessorProvider
    {
        EventProcessor[] GetEventProcessors(
            string teamId,
            string channelId,
            SlackEventType slackEventType,
            CancellationToken cancellationToken);

        void SaveEventProcessingConfiguration(
            Dictionary<string, TeamEventProcessingConfiguration> eventProcessingConfigurations);
    }
}