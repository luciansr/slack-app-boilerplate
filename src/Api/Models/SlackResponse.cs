using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Api.Models
{
    public class SlackResponse
    {
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
        [EnumMember(Value = "is_channel")] 
        IsChannel,
        [EnumMember(Value = "ephemeral")]
        Ephemeral
    }
}