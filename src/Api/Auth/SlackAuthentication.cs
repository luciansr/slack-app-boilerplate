using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Config;

namespace Api.Auth
{
    public class SlackAuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (VerifySlackOrigin(context))
            {
                base.OnActionExecuting(context);
                return;
            }

            context.Result = new ContentResult
            {
                Content = "not_authorized",
                StatusCode = 200
            };
        }

        private bool VerifySlackOrigin(ActionExecutingContext context)
        {
            var slackConfig = (SlackConfig)context.HttpContext.RequestServices.GetService(typeof(SlackConfig));
            var teamDomain = context.HttpContext.Request.Form["team_domain"].FirstOrDefault();
            var request = context.HttpContext.Request;
        
            var formBody = string.Join("&", request.Form.Select(x => $"{x.Key}={WebUtility.UrlEncode(x.Value.FirstOrDefault())}"));
            
            var slackTime = request?.Headers["X-Slack-Request-Timestamp"];
            string expectedSignature = request?.Headers["X-Slack-Signature"];
            var receivedPayload = $"v0:{slackTime}:{formBody}";

            var payloadBytes = Encoding.UTF8.GetBytes(receivedPayload);

            using (var hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(slackConfig.SlackTokens[teamDomain].SigningSecret)))
            {
                var payloadHexString = $"v0={BitConverter.ToString(hmacSha256.ComputeHash(payloadBytes)).Replace("-", "")}";
                return expectedSignature.ToLower().Equals(payloadHexString, StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }
}