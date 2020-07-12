using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Api;
using Models.Config;
using Newtonsoft.Json;
using Services.Auth;

namespace Api.Auth
{
    public class SlackJsonAuthenticationAttribute : TypeFilterAttribute
    {
        public SlackJsonAuthenticationAttribute() : base(typeof(SlackJsonAuthenticationFilter))
        {
        }
    }
    
    internal class SlackJsonAuthenticationFilter : ActionFilterAttribute
    {
        private readonly IAuthConfigurationRepository _authConfigurationRepository;

        public SlackJsonAuthenticationFilter(
            IAuthConfigurationRepository authConfigurationRepository)
        {
            _authConfigurationRepository = authConfigurationRepository;
        }

        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (await VerifySlackOrigin(context.HttpContext.Request, context.HttpContext.RequestAborted))
            {
                base.OnActionExecuting(context);
                return;
            }

            context.Result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(new
                {
                    error = "Only messages from Slack are allowed."
                }),
                StatusCode = 403
            };
        }

        private async Task<bool> VerifySlackOrigin(HttpRequest request, CancellationToken cancellationToken)
        {
            string body = null;
            
            request.EnableBuffering();


            var stream = new StreamReader(request.Body);
            stream.BaseStream.Seek(0, SeekOrigin.Begin);
            body = await stream.ReadToEndAsync();
            // request.Body.Position = 0;

            var teamId = JsonConvert.DeserializeObject<SlackBaseEventBody>(body)?.TeamId;
            if (string.IsNullOrWhiteSpace(teamId))
            {
                return false;
            }

            var slackTime = request?.Headers["X-Slack-Request-Timestamp"];
            string expectedSignature = request?.Headers["X-Slack-Signature"];
            var receivedPayload = $"v0:{slackTime}:{body}";

            var payloadBytes = Encoding.UTF8.GetBytes(receivedPayload);

            using var hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(_authConfigurationRepository.GetTeamSigningSecretAsync(teamId)));
            var payloadHexString = $"v0={BitConverter.ToString(hmacSha256.ComputeHash(payloadBytes)).Replace("-", "")}";
            return expectedSignature.ToLower().Equals(payloadHexString, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}