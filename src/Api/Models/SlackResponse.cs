using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Api.Models
{
    public class SlackResponse
    {
        [JsonProperty("response_type")]
        public SlackResponseType ResponseType { get; set; }
        public string Text { get; set; }
        public List<SlackAttachment> Attachments { get; set; }
        
    }

    public class SlackAttachment
    {
        public string Text { get; set; }
    }

    public enum SlackResponseType
    {
        [EnumMember(Value = "in_channel")] 
        InChannel,
        [EnumMember(Value = "ephemeral")]
        Ephemeral
    }
}