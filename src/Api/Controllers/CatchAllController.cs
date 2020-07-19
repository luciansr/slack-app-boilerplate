using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Auth;
using Api.Middleware;
using Microsoft.AspNetCore.Mvc;
using Models.Api;
using Services.Slack;

namespace Api.Controllers
{

    // /[command] help
    //catch all routes
    [Route("api/{*.}")]
    [ApiController]
    [SlackFormAuthentication]
    public class CatchAllController : ControllerBase
    {
        // /[command] *
        [HttpPost]
        public async Task<IActionResult> Post([FromServices]ISlackClient slackClient, [FromForm]SlackRequest slackRequest, CancellationToken cancellationToken)
        {
            await slackClient.PostOnChannelAsync(slackRequest.TeamDomain, slackRequest.ChannelId, "command [not_found]", cancellationToken);
            return Ok(new SlackResponse
            {
                ResponseType = SlackResponseType.InChannel,
                Text = $"Sorry. [{slackRequest.Command} {slackRequest.Text}] is not a valid command. 😟" +
                       $"Try using [{slackRequest.Command} help] instead."
            });
        }
    }
}
