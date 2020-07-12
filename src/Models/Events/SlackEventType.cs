using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Models.Events
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SlackEventType
    {
        [EnumMember(Value = "Message")] 
        Message,
        [EnumMember(Value = "UserJoined")]
        UserJoined,
        [EnumMember(Value = "AppMention")]
        AppMention,
        [EnumMember(Value = "ReactionAdded")]
        ReactionAdded
    }
}