﻿using System;
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
            return Ok(new SlackResponse
            {
                ResponseType = SlackResponseType.InChannel,
                Text = "teste"
            });
        }
        
        // /[command] list help [extra parameters from slack]
        [HttpPost]
        [Route("help")]
        public async Task<IActionResult> Help([FromServices]SlackClient slackClient, [FromForm]SlackRequest slackRequest, [FromQuery]string extraParameters, CancellationToken cancellationToken)
        {
            await slackClient.PostOnChannelAsync(slackRequest.team_domain, slackRequest.channel_id, $"command [list help] parameters: [{extraParameters}]", cancellationToken);
            return Ok(new SlackResponse
            {
                ResponseType = SlackResponseType.InChannel,
                Text = String.Join(Environment.NewLine, new List<string>
                {
                    $"Hey, there!",
                    $"This is the [{slackRequest.command} list]. With it you can"
                }),
                Attachments = new List<SlackAttachment>
                {
                    new SlackAttachment
                    {
                        Text = $"List flags: Try [{slackRequest.command} list flags] list all available flags."
                    }
                }
            });
        }
    }
}
