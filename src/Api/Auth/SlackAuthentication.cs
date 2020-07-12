using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Config;
using Newtonsoft.Json;

namespace Api.Auth
{
    
    public class SlackAuthenticationAttribute : TypeFilterAttribute
    {
        public SlackAuthenticationAttribute() : base(typeof(SlackAuthenticationFilter))
        {
        }
    }
    
    internal class SlackAuthenticationFilter : ActionFilterAttribute
    {
        private readonly SlackConfig _slackConfig;

        public SlackAuthenticationFilter(SlackConfig slackConfig)
        {
            _slackConfig = slackConfig;
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (VerifySlackOrigin(context.HttpContext.Request))
            {
                base.OnActionExecuting(context);
                return;
            }

            context.Result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(new
                {
                    error = "Only messages from Slack are allowed!"
                }),
                StatusCode = 403
            };
        }

        private bool VerifySlackOrigin(HttpRequest request)
        {
            var teamDomain = request.Form["team_domain"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(teamDomain))
            {
                return false;
            }

            var body = string.Join("&", request.Form.Select(x => $"{x.Key}={WebUtility.UrlEncode(x.Value.FirstOrDefault())}"));

            if (string.IsNullOrEmpty(body))
            {
                request.EnableBuffering();
                var stream = new StreamReader(request.Body, Encoding.UTF8);
                body = stream.ReadToEnd();
                request.Body.Position = 0;
            }

            var slackTime = request?.Headers["X-Slack-Request-Timestamp"];
            string expectedSignature = request?.Headers["X-Slack-Signature"];
            var receivedPayload = $"v0:{slackTime}:{body}";

            var payloadBytes = Encoding.UTF8.GetBytes(receivedPayload);

            using var hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(_slackConfig.SlackTokens[teamDomain].SigningSecret));
            var payloadHexString = $"v0={BitConverter.ToString(hmacSha256.ComputeHash(payloadBytes)).Replace("-", "")}";
            return expectedSignature.ToLower().Equals(payloadHexString, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}