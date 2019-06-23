using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Controllers.SlackBase;
using Api.Middleware;
using Clients.Slack;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ListController : SlackBaseController
    {
        public ListController(SlackScopedClient slackScopedClient)
            : base(slackScopedClient)
        {
            
        }
        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SlackRequest slackRequest, [FromServices]SlackCustomClient slackClient)
        {
            await SetClient();
            Console.WriteLine("test");
            await slackClient.PostOnChannelAsync(slackRequest.channel_id, "test message");
            return Ok();
        }

        // POST api/values
//        [HttpPost]
//        public void Post([FromBody]SlackRequest request)
//        {
//            Console.WriteLine(request.text);
//        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
