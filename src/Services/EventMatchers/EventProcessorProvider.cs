using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Models.Events;

namespace Services.EventMatchers
{
    public class EventProcessorProvider : IEventProcessorProvider
    {
        private readonly Dictionary<string, Dictionary<string, EventProcessor[]>> _eventProcessors;

        public EventProcessorProvider()
        {
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
                            result = result.Concat(ownEventProcessors).ToArray();
                        }
                    }
                }
            }

            return result;
        }

        public void SaveEventProcessingConfiguration(
            Dictionary<string, TeamProcessingConfiguration> eventProcessingConfigurations)
        {
            throw new System.NotImplementedException();
        }
    }
}