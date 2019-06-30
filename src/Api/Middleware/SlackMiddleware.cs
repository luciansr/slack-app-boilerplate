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
        public string enterprise_id { get; set; }
        public string enterprise_name { get; set; }
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
        }
        
        public async Task Invoke(HttpContext httpContext)
        {
            var textCommand = httpContext.Request.Form["text"].FirstOrDefault()?.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);

            if (textCommand == null || textCommand.Length == 0)
            {
                //TODO Log error
                return;
            }

            httpContext.Request.Path = $"/api/{textCommand.FirstOrDefault()}";
            await _next(httpContext);
        }
    }
}