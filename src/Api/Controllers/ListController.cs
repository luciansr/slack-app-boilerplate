﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        // POST api/values
        [HttpPost]
        public void Post([FromBody]SlackRequest slackRequest)
        {
            Console.WriteLine("test");
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
