using System;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Api;
using Newtonsoft.Json;
using Services.Auth;

namespace Api.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class SlackAuthorizeAttribute :  AuthorizeAttribute
    {
        public SlackAuthorizeAttribute()
        {
            AuthenticationSchemes = SlackAuthenticationHandler.AuthenticationScheme;
        }
    }
    
    public class SlackAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAuthConfigurationRepository _authConfigurationRepository;
        public static string AuthenticationScheme = "slack_json_auth"; 
        public static string AuthenticationName = "slack_json_auth_name"; 
        
        public SlackAuthenticationHandler(
            IWebHostEnvironment hostEnvironment,
            IAuthConfigurationRepository authConfigurationRepository,
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
            _hostEnvironment = hostEnvironment;
            _authConfigurationRepository = authConfigurationRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                return AuthenticateResult.NoResult();
            }

            if (!await VerifySlackOrigin(Context.Request, Context.RequestAborted))
            {
                return AuthenticateResult.Fail("Only requests from Slack are valid.");
            }

            return AuthenticateResult.Success(
                new AuthenticationTicket(
                    new ClaimsPrincipal(
                        new ClaimsIdentity(AuthenticationScheme)),
                    AuthenticationScheme));
        }
        
        private async Task<bool> VerifySlackOrigin(HttpRequest request, CancellationToken cancellationToken)
        {
            if (_hostEnvironment.IsDevelopment())
            {
                return true;
            }

            request.EnableBuffering();
            var stream = new StreamReader(request.Body);
            stream.BaseStream.Seek(0, SeekOrigin.Begin);
            var body = await stream.ReadToEndAsync();
            request.Body.Position = 0;

            var teamId = JsonConvert.DeserializeObject<SlackBaseEventBody>(body)?.TeamId;
            if (string.IsNullOrWhiteSpace(teamId))
            {
                return false;
            }

            var slackTime = request.Headers["X-Slack-Request-Timestamp"];
            string expectedSignature = request.Headers["X-Slack-Signature"];
            var receivedPayload = $"v0:{slackTime}:{body}";

            var payloadBytes = Encoding.UTF8.GetBytes(receivedPayload);

            using var hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(_authConfigurationRepository.GetTeamSigningSecretAsync(teamId)));
            var payloadHexString = $"v0={BitConverter.ToString(hmacSha256.ComputeHash(payloadBytes)).Replace("-", "")}";
            return expectedSignature.ToLower().Equals(payloadHexString, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}