using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SlackController : ControllerBase
    {
        // POST api/values
        [HttpPost]
        public void Post(
            [FromForm]string token,
            [FromForm]string team_id,
            [FromForm]string team_domain,
            [FromForm]string channel_id,
            [FromForm]string channel_name,
            [FromForm]string user_id,
            [FromForm]string user_name,
            [FromForm]string command,
            [FromForm]string text,
            [FromForm]string response_url,
            [FromForm]string trigger_id
            )
        {
            Console.WriteLine(text);
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
