using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private readonly Dictionary<string, Dictionary<string, EventProcessor[]>> _eventProcessors;

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
            _eventProcessors = new Dictionary<string, Dictionary<string, EventProcessor[]>>();
        }
        
        public EventProcessor[] GetEventProcessors(
            string teamId, 
            string channelId, 
            SlackEventType slackEventType, 
            CancellationToken cancellationToken)
        {
            var result = new EventProcessor[0];

            if (_eventProcessors != null
                && _eventProcessors.TryGetValue(teamId, out var channelEventProcessors))
            {
                if (channelEventProcessors != null)
                {
                    if (channelEventProcessors.TryGetValue(channelId, out var ownEventProcessors))
                    {
                        if (ownEventProcessors != null)
                        {
                            result = result.Concat(ownEventProcessors).ToArray();
                        }
                    }
                    
                    if (channelEventProcessors.TryGetValue("*", out var genericEventProcessors))
                    {
                        if (ownEventProcessors != null)
                        {
                            result = result.Concat(genericEventProcessors).ToArray();
                        }
                    }
                }
            }

            return result;
        }

        public void SaveEventProcessingConfiguration(
            Dictionary<string, TeamProcessingConfiguration> eventProcessingConfigurations)
        {
            if (eventProcessingConfigurations == null)
            {
                return;
            }

            var newEventProcessors = new Dictionary<string, Dictionary<string, EventProcessor[]>>();
            
            foreach (var item in eventProcessingConfigurations)
            {
                newEventProcessors.Add(item.Key, new Dictionary<string, EventProcessor[]>());

                if (item.Value.ChannelConfigurations == null)
                {
                    continue;
                }

                foreach (var channelItem in item.Value.ChannelConfigurations)
                {
                    var eventProcessors = new EventProcessor[channelItem.Value.Length];

                    for (var i = 0; i < channelItem.Value.Length; i++)
                    {
                        eventProcessors[i] = GetEventProcessor(channelItem.Value[i]);
                    }

                    newEventProcessors[item.Key].Add(channelItem.Key, eventProcessors);
                }
            }
        }

        private EventProcessor GetEventProcessor(ChannelProcessingConfiguration processingConfiguration)
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