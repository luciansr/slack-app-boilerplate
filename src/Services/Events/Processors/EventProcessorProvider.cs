using System.Linq;
using Models.Events;

namespace Services.Events.Processors
{
    public class EventProcessorProvider : IEventProcessorProvider
    {
        private readonly IEventMatcherFactory _eventMatcherFactory;
        private readonly IActionExecutorFactory _actionExecutorFactory;
        private SlackProcessingState _eventProcessors;

        public EventProcessorProvider(IEventMatcherFactory eventMatcherFactory, IActionExecutorFactory actionExecutorFactory)
        {
            _eventMatcherFactory = eventMatcherFactory;
            _actionExecutorFactory = actionExecutorFactory;
        }

        public EventProcessor[] GetEventProcessors(
            string teamId,
            string channelId,
            SlackEventType slackEventType)
        {
            var result = new EventProcessor[0];

            if (_eventProcessors?.TeamConfigurations != null
                && _eventProcessors.TeamConfigurations.TryGetValue(teamId, out var teamEventProcessors))
            {
                return GetEventProcessorsForChannel(channelId, slackEventType, teamEventProcessors);
            }

            return result;
        }

        private EventProcessor[] GetEventProcessorsForChannel(string channelId, SlackEventType slackEventType, TeamProcessingConfiguration<EventProcessor> eventProcessors)
        {
            var result = new EventProcessor[0];

            if (eventProcessors?.ChannelConfigurations == null)
            {
                return result;
            }

            if (eventProcessors.ChannelConfigurations.TryGetValue(channelId, out var ownEventProcessors))
            {
                if (ownEventProcessors != null)
                {
                    result = result.Concat(GetEventProcessorsForSlackEventType(slackEventType, ownEventProcessors)).ToArray();
                }
            }

            if (eventProcessors.ChannelConfigurations.TryGetValue("*", out var genericEventProcessors))
            {
                if (genericEventProcessors != null)
                {
                    result = result.Concat(GetEventProcessorsForSlackEventType(slackEventType, genericEventProcessors)).ToArray();
                }
            }

            return result;
        }

        private EventProcessor[] GetEventProcessorsForSlackEventType(SlackEventType slackEventType, ChannelProcessingConfiguration<EventProcessor> eventProcessors)
        {
            var result = new EventProcessor[0];

            if (eventProcessors?.EventConfigurations == null)
            {
                return result;
            }

            if (eventProcessors.EventConfigurations.TryGetValue(slackEventType.ToString(), out var ownEventProcessors))
            {
                if (ownEventProcessors != null)
                {
                    result = result.Concat(ownEventProcessors).ToArray();
                }
            }

            if (eventProcessors.EventConfigurations.TryGetValue("*", out var genericEventProcessors))
            {
                if (genericEventProcessors != null)
                {
                    result = result.Concat(genericEventProcessors).ToArray();
                }
            }

            return result;
        }

        public void SaveEventProcessingConfiguration(
            SlackProcessingConfiguration slackProcessingConfigurations)
        {
            if (slackProcessingConfigurations == null)
            {
                return;
            }

            var slackProcessingState = new SlackProcessingState
            {
                TeamConfigurations = slackProcessingConfigurations?.TeamConfigurations
                    .ToDictionary(
                        x => x.Key,
                        x => new TeamProcessingConfiguration<EventProcessor>
                        {
                            ChannelConfigurations = x.Value.ChannelConfigurations?.ToDictionary(
                                y => y.Key,
                                y => new ChannelProcessingConfiguration<EventProcessor>
                                {
                                    EventConfigurations = y.Value.EventConfigurations?.ToDictionary(
                                        z => z.Key,
                                        z => z.Value.Select(GetEventProcessor).ToArray()
                                    )
                                }
                            )
                        })
            };

            _eventProcessors = slackProcessingState;
        }

        private EventProcessor GetEventProcessor(ProcessingConfiguration processingConfiguration)
        {
            return new EventProcessor(
                _eventMatcherFactory.GetEventMatcher(processingConfiguration.Match), _actionExecutorFactory.GetActionExecutor(processingConfiguration.Action));
        }
    }
}