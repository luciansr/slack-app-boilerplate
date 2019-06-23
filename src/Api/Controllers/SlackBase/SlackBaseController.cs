using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Clients.Slack;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.SlackBase
{
    public class SlackBaseController : ControllerBase
    {
        private readonly SlackScopedClient _slackScopedClient;

        public SlackBaseController(SlackScopedClient slackScopedClient)
        {
            _slackScopedClient = slackScopedClient;
        }

        protected async Task SetClient()
        {
            if(Request.Form.TryGetValue("team_domain", out var teamDomain))
            {
                using (var tokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(2)))
                {
                    await _slackScopedClient.SetTeam(teamDomain.FirstOrDefault()?.Trim(), tokenSource.Token);
                }
            }
        }
    }
}