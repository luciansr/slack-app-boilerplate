using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Models.Events
{
    public class TeamProcessingConfiguration
    {
        [JsonProperty("teamId")]
        public string TeamId { get; set; }
        [JsonProperty("channelConfigurations")]
        public Dictionary<string, ChannelProcessingConfiguration[]> ChannelConfigurations { get; set; }
    }

    public class ChannelProcessingConfiguration
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
        [JsonProperty("data")]
        public string Data { get; set; }
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
        [JsonProperty("text")]
        public string Text { get; set; }
    }

  
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActionType
    {
        [EnumMember(Value = "AnswerToChannelMessage")]
        AnswerToChannelMessage,
    }
}