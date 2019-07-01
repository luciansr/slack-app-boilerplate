using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Auth;
using Api.Middleware;
using Clients.Slack;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    // /[command] list
    [Route("api/[controller]")]
    [ApiController]
    [SlackAuthentication]
    [SlackAuthorize(authorizedUsernames: new[] {"luciansr"})]
    public class ListController : ControllerBase
    {
        // /[command] list
        [HttpPost]
        //catch all routes
        [Route("{*.}")] 
        public async Task<IActionResult> Post([FromServices]SlackClient slackClient, [FromForm]SlackRequest slackRequest, CancellationToken cancellationToken)
        {
            await slackClient.PostOnChannelAsync(slackRequest.team_domain, slackRequest.channel_id, "command [list]", cancellationToken);
            return Ok();
        }
        
        // /[command] list help [extra parameters from slack]
        [HttpPost]
        [Route("help")]
        public async Task<IActionResult> Help([FromServices]SlackClient slackClient, [FromForm]SlackRequest slackRequest, [FromQuery]string extraParameters, CancellationToken cancellationToken)
        {
            await slackClient.PostOnChannelAsync(slackRequest.team_domain, slackRequest.channel_id, $"command [list help] parameters: [{extraParameters}]", cancellationToken);
            return Ok();
        }
    }
}
