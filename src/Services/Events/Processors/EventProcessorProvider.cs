using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualBasic;
using Models.Events;
using Services.Events.Actions;
using Services.Events.Matchers;

namespace Services.Events.Processors
{
    public class EventProcessorProvider : IEventProcessorProvider
    {
        private readonly UnknownActionExecutor _unknownActionExecutor;
        private readonly AnswerMessageActionExecutor _answerMessageActionExecutor;
        private readonly UnknownEventMatcher _unknownEventMatcher;
        private readonly TextContainsEventMatcher _textContainsEventMatcher;
        private SlackProcessingState _eventProcessors;

        public EventProcessorProvider(
            UnknownActionExecutor unknownActionExecutor,
            AnswerMessageActionExecutor answerMessageActionExecutor,
            UnknownEventMatcher unknownEventMatcher,
            TextContainsEventMatcher textContainsEventMatcher)
        {
            _unknownActionExecutor = unknownActionExecutor;
            _answerMessageActionExecutor = answerMessageActionExecutor;
            _unknownEventMatcher = unknownEventMatcher;
            _textContainsEventMatcher = textContainsEventMatcher;
        }
        
        public EventProcessor[] GetEventProcessors(
            string teamId, 
            string channelId, 
            SlackEventType slackEventType, 
            CancellationToken cancellationToken)
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
                if (ownEventProcessors != null)
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
                if (ownEventProcessors != null)
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
            return new EventProcessor(GetEventMatcher(processingConfiguration.Match), GetActionExecutor(processingConfiguration.Action));
        }
        
        private IEventMatcher GetEventMatcher(EventMatchConfiguration matchConfiguration)
        {
            return matchConfiguration switch  {
                {Type: MatchType.TextContains} => _textContainsEventMatcher,
                _ => _unknownEventMatcher
            };
        }

        private IActionExecutor GetActionExecutor(ActionConfiguration actionConfiguration)
        {
            return actionConfiguration switch
            {
                {Type: ActionType.AnswerToMessage} => _answerMessageActionExecutor,
                _ => _unknownActionExecutor
            };
        }
    }
}