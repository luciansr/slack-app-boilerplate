using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Clients.Slack;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;

namespace Api.Middleware
{


    public class SlackMiddleware
    {
        private readonly RequestDelegate _next;

        public SlackMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var textCommand = httpContext.Request.Form["text"].FirstOrDefault()?.Split(new[] {" "}, 3, StringSplitOptions.RemoveEmptyEntries);

            if (textCommand == null || textCommand.Length == 0)
            {
                await _next(httpContext);
                return;
            }

            var path = $"/api/{textCommand.FirstOrDefault()}";

            if (textCommand.Length > 1)
            {
                path += $"/{textCommand[1]}";
            }

            if (textCommand.Length > 2)
            {
                httpContext.Request.QueryString = httpContext.Request.QueryString.Add("extraParameters", textCommand[2]);
            }

            httpContext.Request.Path = path;
            await _next(httpContext);
        }
    }
}