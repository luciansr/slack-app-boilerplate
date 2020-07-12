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
using Services.Auth;

namespace Api.Auth
{
    
    public class SlackFormAuthenticationAttribute : TypeFilterAttribute
    {
        public SlackFormAuthenticationAttribute() : base(typeof(SlackFormAuthenticationFilter))
        {
        }
    }
    
    internal class SlackFormAuthenticationFilter : ActionFilterAttribute
    {
        private readonly IAuthConfigurationRepository _authRepository;

        public SlackFormAuthenticationFilter(IAuthConfigurationRepository authRepository)
        {
            _authRepository = authRepository;
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
                    error = "Only messages from Slack are allowed"
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

            var slackTime = request?.Headers["X-Slack-Request-Timestamp"];
            string expectedSignature = request?.Headers["X-Slack-Signature"];
            var receivedPayload = $"v0:{slackTime}:{body}";

            var payloadBytes = Encoding.UTF8.GetBytes(receivedPayload);

            using var hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(_authRepository.GetTeamSigningSecretAsync(teamDomain)));
            var payloadHexString = $"v0={BitConverter.ToString(hmacSha256.ComputeHash(payloadBytes)).Replace("-", "")}";
            return expectedSignature.ToLower().Equals(payloadHexString, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}