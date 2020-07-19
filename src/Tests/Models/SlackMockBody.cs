using Models.Api;
using Newtonsoft.Json;

namespace Tests.Models
{
    public class SlackMockBody
    {
        [JsonProperty("body")]
        public SlackEventBody Body { get; set;  }
    }
}