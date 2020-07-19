using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Models.Events
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SlackEventType
    {
        [EnumMember(Value = nameof(Unknown))] 
        Unknown,
        [EnumMember(Value = nameof(Message))] 
        Message,
        [EnumMember(Value = nameof(ThreadMessage))] 
        ThreadMessage,
        [EnumMember(Value = nameof(ChannelJoin))]
        ChannelJoin,
        [EnumMember(Value = nameof(AppMention))]
        AppMention,
        [EnumMember(Value = nameof(ReactionAdded))]
        ReactionAdded
    }
}