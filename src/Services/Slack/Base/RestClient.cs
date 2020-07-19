using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Services.Slack.Base
{
    public abstract class RestClient
    {
        private readonly HttpClient _httpClient;

        protected RestClient(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(baseUrl);
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

        protected async Task<HttpResponseMessage> GetAsync(
            string url,
            CancellationToken cancellationToken,
            Dictionary<string, string> httpHeaders = null,
            int timeoutInMilliseconds = -1)
        {
            return await SendAsStringAsync(HttpMethod.Get, url, cancellationToken, httpHeaders);
        }

        protected  async Task<HttpResponseMessage> PostAsJsonAsync<TType>(
            string url,
            TType contentObject,
            CancellationToken cancellationToken,
            Dictionary<string, string> httpHeaders = null)
        {
            return await SendAsStringAsync(HttpMethod.Post, url, cancellationToken, httpHeaders, GetStringFromObject(contentObject));
        }

        protected  string GetStringFromObject<TType>(TType contentObject)
        {
            return JsonConvert.SerializeObject(contentObject);
        }

        protected  TType GetObjectFromString<TType>(string content)
        {
            return JsonConvert.DeserializeObject<TType>(content);
        }
    }
}