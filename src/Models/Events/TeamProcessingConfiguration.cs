using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Models.Events
{
    public class SlackProcessingConfiguration : BaseSlackProcessingConfiguration<ProcessingConfiguration>
    {
        
    }

    public class BaseSlackProcessingConfiguration<TEventConfiguration>
    {
        [JsonProperty("teamConfigurations")]
        public Dictionary<string, TeamProcessingConfiguration<TEventConfiguration>> TeamConfigurations { get; set; }
    }
    
    public class TeamProcessingConfiguration<TEventConfiguration>
    {
        [JsonProperty("channelConfigurations")]
        public Dictionary<string, ChannelProcessingConfiguration<TEventConfiguration>> ChannelConfigurations { get; set; }
    }
    
    public class ChannelProcessingConfiguration<TEventConfiguration>
    {
        [JsonProperty("eventConfigurations")]
        public Dictionary<string, TEventConfiguration[]> EventConfigurations { get; set; }
    }

    public class ProcessingConfiguration
    {
        [JsonProperty("match")]
        public EventMatchConfiguration Match { get; set; }
        [JsonProperty("action")]
        public ActionConfiguration Action { get; set; }
    }

    public class EventMatchConfiguration
    {
        [JsonProperty("type")]
        public MatchType Type { get; set; }
        [JsonProperty("target")]
        public string Target { get; set; }
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MatchType
    {
        [EnumMember(Value = "TextContains")]
        TextContains,
    }
    
    public class ActionConfiguration
    {
        [JsonProperty("type")]
        public ActionType Type { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }

  
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActionType
    {
        [EnumMember(Value = "AnswerToMessage")]
        AnswerToMessage,
    }
}