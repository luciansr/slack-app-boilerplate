using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Clients.Slack;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace Api.Middleware
{
    public class SlackRequest {
        public string token { get; set; }
        public string team_id { get; set; }
        public string team_domain { get; set; }
        public string channel_id { get; set; }
        public string channel_name { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string command { get; set; }
        public string text { get; set; }
        public string response_url { get; set; }
        public string trigger_id { get; set; }
    }
    
    public class SlackMiddleware
    {
        private readonly RequestDelegate _next;

        public SlackMiddleware(RequestDelegate next)
        {
            _next = next;
//            _slackScopedClient = slackScopedClient;
        }
        
        public async Task Invoke(HttpContext httpContext)
        {
            var formDictionary = httpContext.Request.Form.ToDictionary(item => item.Key, item => item.Value.FirstOrDefault()?.Trim());
            var jsonRequest = JObject.FromObject(formDictionary);
            var request = jsonRequest.ToObject<SlackRequest>();
            var textCommand = request.text.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);

//            await _slackScopedClient.SetTeam(request.team_domain, cancellationToken);
            // Store the "pre-modified" response stream.
            var existingBody = httpContext.Response.Body;

            using (var newBody = new MemoryStream())
            {
                httpContext.Request.Path = $"/api/{textCommand.FirstOrDefault()}";
                httpContext.Request.Body.Flush();
                await newBody.WriteAsync(Encoding.UTF8.GetBytes(jsonRequest.ToString()), 0, Encoding.UTF8.GetBytes(jsonRequest.ToString()).Length);
                httpContext.Request.Body = newBody;
                newBody.Seek(0, SeekOrigin.Begin);
                httpContext.Request.ContentType = "application/json";
                await _next(httpContext);

                // Set the stream back to the original.
                httpContext.Response.Body = existingBody;

                newBody.Seek(0, SeekOrigin.Begin);

                // Previous code removed for brevity.
            }
            
            
        }
    }
}