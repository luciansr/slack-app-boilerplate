using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Auth;
using Api.Middleware;
using Api.Models;
using Clients.Slack;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    // /[command] help
    [Route("api/[controller]")]
    [ApiController]
    [SlackAuthentication]
    public class HelpController : ControllerBase
    {
        // /[command] help
        [HttpPost]
        //catch all routes
        [Route("{*.}")]
        public async Task<IActionResult> Post([FromServices]SlackClient slackClient, [FromForm]SlackRequest slackRequest, CancellationToken cancellationToken)
        {
            await slackClient.PostOnChannelAsync(slackRequest.team_domain, slackRequest.channel_id, "command [help]", cancellationToken);
            return Ok(new SlackResponse
            {
                ResponseType = SlackResponseType.InChannel,
                Text = String.Join(Environment.NewLine, new List<string>
                {
                    $"Welcome to [{slackRequest.command} help]!",
                    $"The available commands are:"
                }),
                Attachments = new List<SlackAttachment>
                {
                    new SlackAttachment
                    {
                        Text = $"List: Try [{slackRequest.command} list help] to know details."
                    }
                }
            });
        }
    }
}
