using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Models.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Clients.Slack
{
    public class SlackPostToChannelRequest
    {
        public string Text { get; set; }
        public string Token { get; set; }
        public string Channel { get; set; }
    }

    public class SlackClientResponse
    {
        public bool Ok { get; set; }
    }

    public class SlackClient
    {
        private readonly HttpClient _httpClient;
        private readonly SlackConfig _slackConfig;

        public SlackClient(HttpClient httpClient, SlackConfig slackConfig)
        {
            _httpClient = httpClient;
            _slackConfig = slackConfig;
            _httpClient.BaseAddress = new Uri("https://slack.com");
        }

        public async Task PostOnChannelAsync(string teamDomain, string channelId, string message, CancellationToken cancellationToken)
        {
            var token = _slackConfig.SlackTokens[teamDomain].BotToken;
            var response = await PostAsJsonAsync<Object>("/api/chat.postMessage", new SlackPostToChannelRequest
            {
                Channel = channelId,
                Text = message
            }, cancellationToken,new Dictionary<string, string>
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
        
        protected virtual async Task<HttpResponseMessage> SendAsStringAsync(
            HttpMethod httpMethod,
            string url,
            CancellationToken cancellationToken,
            Dictionary<string, string> httpHeaders = null,
            string content = null
        )
        {
            var httpRequestMessage = new HttpRequestMessage(httpMethod, url);

            if (!string.IsNullOrWhiteSpace(content))
            {
                httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }

            if (httpHeaders != null)
            {
                foreach (var item in httpHeaders)
                {
                    httpRequestMessage.Headers.Add(item.Key, item.Value);
                }
            }

            var sendRequestAsync = new Func<CancellationToken, Task<HttpResponseMessage>>(token => _httpClient.SendAsync(httpRequestMessage, token));

            return await sendRequestAsync(cancellationToken);
        }

        private async Task<HttpResponseMessage> GetAsync(
            string url,
            CancellationToken cancellationToken,
            Dictionary<string, string> httpHeaders = null,
            int timeoutInMilliseconds = -1)
        {
            return await SendAsStringAsync(HttpMethod.Get, url, cancellationToken, httpHeaders);
        }

        private async Task<HttpResponseMessage> PostAsJsonAsync<TType>(
            string url,
            TType contentObject,
            CancellationToken cancellationToken,
            Dictionary<string, string> httpHeaders = null)
        {
            return await SendAsStringAsync(HttpMethod.Post, url, cancellationToken, httpHeaders, GetStringFromObject(contentObject));
        }
        
        private string GetStringFromObject<TType>(TType contentObject)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.SerializeObject(contentObject, jsonSerializerSettings);
        }
        
        private TType GetObjectFromString<TType>(string content)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.DeserializeObject<TType>(content, jsonSerializerSettings);
        }
    }
}