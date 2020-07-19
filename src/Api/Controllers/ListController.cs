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

    // /[command] list
    [Route("api/[controller]")]
    [ApiController]
    [SlackFormAuthentication]
    [SlackAuthorizeByUser(authorizedUsernames: new[] {"luciansr"})]
    public class ListController : ControllerBase
    {
        // /[command] list
        [HttpPost]
        //catch all routes
        [Route("{*.}")] 
        public async Task<IActionResult> Post([FromServices]ISlackClient slackClient, [FromForm]SlackRequest slackRequest, CancellationToken cancellationToken)
        {
            await slackClient.PostOnChannelAsync(slackRequest.TeamDomain, slackRequest.ChannelId, "command [list]", cancellationToken);
            return Ok(new SlackResponse
            {
                ResponseType = SlackResponseType.InChannel,
                Text = "teste"
            });
        }
        
        // /[command] list help [extra parameters from slack]
        [HttpPost]
        [Route("help")]
        public async Task<IActionResult> Help([FromServices]ISlackClient slackClient, [FromForm]SlackRequest slackRequest, [FromQuery]string extraParameters, CancellationToken cancellationToken)
        {
            await slackClient.PostOnChannelAsync(slackRequest.TeamDomain, slackRequest.ChannelId, $"command [list help] parameters: [{extraParameters}]", cancellationToken);
            return Ok(new SlackResponse
            {
                ResponseType = SlackResponseType.InChannel,
                Text = String.Join(Environment.NewLine, new List<string>
                {
                    $"Hey, there!",
                    $"This is the [{slackRequest.Command} list]. With it you can"
                }),
                Attachments = new List<SlackAttachment>
                {
                    new SlackAttachment
                    {
                        Text = $"List flags: Try [{slackRequest.Command} list flags] list all available flags."
                    }
                }
            });
        }
    }
}
