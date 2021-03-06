using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Api.Auth;
using Microsoft.AspNetCore.Mvc;
using Models.Api;
using Services;

namespace Api.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly SlackEventProducer _slackEventProducer;

        public EventsController(SlackEventProducer slackEventProducer)
        {
            _slackEventProducer = slackEventProducer;
        }

        [HttpPost]
        [Route("receive")]
        [SlackAuthorize]
        public async Task<IActionResult> Receive(
            [FromBody, Required] 
            SlackEventBody slackEventBody,
            CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(slackEventBody.Challenge))
            {
                return Ok(slackEventBody.Challenge);
            }

            await _slackEventProducer.ReceiveSlackEvent(slackEventBody, cancellationToken);
            return Ok();
        }
    }
}