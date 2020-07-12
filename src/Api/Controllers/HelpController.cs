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
    [Route("api/[controller]")]
    [ApiController]
    [SlackFormAuthentication]
    public class HelpController : ControllerBase
    {
        // /[command] help
        [HttpPost]
        //catch all routes
        [Route("{*.}")]
        public async Task<IActionResult> Post([FromServices]SlackClient slackClient, [FromForm]SlackRequest slackRequest, CancellationToken cancellationToken)
        {
            await slackClient.PostOnChannelAsync(slackRequest.TeamDomain, slackRequest.ChannelId, "command [help]", cancellationToken);
            return Ok(new SlackResponse
            {
                ResponseType = SlackResponseType.InChannel,
                Text = string.Join(Environment.NewLine, new List<string>
                {
                    $"Welcome to [{slackRequest.Command} help]!",
                    $"The available commands are:"
                }),
                Attachments = new List<SlackAttachment>
                {
                    new SlackAttachment
                    {
                        Text = $"List: Try [{slackRequest.Command} list help] to know details."
                    }
                }
            });
        }
    }
}
