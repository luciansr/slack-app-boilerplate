using Models.Events;

namespace Services.Events.Processors
{
    public interface IEventProcessorProvider
    {
        EventProcessor[] GetEventProcessors(
            string teamId,
            string channelId,
            SlackEventType slackEventType);

        void SaveEventProcessingConfiguration(
            SlackProcessingConfiguration slackProcessingConfigurations);
    }
}