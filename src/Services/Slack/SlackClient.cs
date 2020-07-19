using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Services.Auth;
using Services.Slack.Base;

namespace Services.Slack
{
    public class SlackPostToChannelRequest
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }
    }

    public class SlackClientResponse
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }
    }

    public class SlackClient : RestClient
    {
        private readonly IAuthConfigurationRepository _authRepository;

        public SlackClient(HttpClient httpClient, IAuthConfigurationRepository authRepository) 
            : base(httpClient, "https://slack.com")
        {
            _authRepository = authRepository;
        }

        public async Task PostOnChannelAsync(string teamDomain, string channelId, string message, CancellationToken cancellationToken)
        {
            var token = _authRepository.GetTeamBotTokenAsync(teamDomain);
            var response = await PostAsJsonAsync(
                "/api/chat.postMessage",
                new SlackPostToChannelRequest
                {
                    Channel = channelId,
                    Text = message
                },
                cancellationToken,
                new Dictionary<string, string>
                {
                    {"Authorization", $"Bearer {token}"}
                });

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var slackResponse = GetObjectFromString<SlackClientResponse>(responseString);
                Console.WriteLine();
            }
        }

    }
}