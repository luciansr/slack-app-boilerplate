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

    // /[command] help
    //catch all routes
    [Route("api/{*.}")]
    [ApiController]
    [SlackAuthentication]
    public class CatchAllController : ControllerBase
    {
        // /[command] *
        [HttpPost]
        public async Task<IActionResult> Post([FromServices]SlackClient slackClient, [FromForm]SlackRequest slackRequest, CancellationToken cancellationToken)
        {
            await slackClient.PostOnChannelAsync(slackRequest.team_domain, slackRequest.channel_id, "command [not_found]", cancellationToken);
            return Ok();
        }
    }
}
