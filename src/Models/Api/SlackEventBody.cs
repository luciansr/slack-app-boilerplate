using Newtonsoft.Json;

namespace Models.Api
{
    public class SlackBaseEventBody
    {
        [JsonProperty("team_id")]
        public string TeamId { get; set; }
    }

    public class SlackEventBody : SlackBaseEventBody
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("challenge")]
        public string Challenge { get; set; }

        [JsonProperty("api_app_id")]
        public string ApiAppId { get; set; }

        [JsonProperty("event")]
        public SlackEvent Event { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("event_time")]
        public long EventTime { get; set; }

        [JsonProperty("authed_users")]
        public string[] AuthedUsers { get; set; }
    }

    public class SlackEvent
    {
        [JsonProperty("client_msg_id")]
        public string ClientMsgId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("subtype")]
        public string Subtype { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("ts")]
        public string MessageIdentifier { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("event_ts")]
        public double EventTs { get; set; }

        [JsonProperty("channel_type")]
        public string ChannelType { get; set; }

        [JsonProperty("thread_ts")]
        public string ThreadParentMessageIdentifier { get; set; }

        [JsonProperty("parent_user_id")]
        public string ParentUserId { get; set; }

        [JsonProperty("blocks")]
        public SlackEventBlock[] Blocks { get; set; }
    }

    public class SlackEventBlock
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("block_id")]
        public string BlockId { get; set; }

        [JsonProperty("elements")]
        public SlackEventBlockElement[] Elements { get; set; }
    }

    public class SlackEventBlockElement
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("elements")]
        public SlackEventBlockInnerElement[] Elements { get; set; }
    }

    public class SlackEventBlockInnerElement
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}