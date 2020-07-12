using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Api.Middleware
{
    public class SlackCommandMiddleware
    {
        private readonly RequestDelegate _next;

        public SlackCommandMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.StartsWithSegments(new PathString("/api/events/receive")))
            {
                await _next(httpContext);
                return;
            }
            
            string[] textCommand = null;
            if (httpContext.Request.Form.TryGetValue("text", out var textFormValue))
            {
                textCommand = textFormValue
                    .FirstOrDefault()
                    ?.Split(new[] {" "}, 3, StringSplitOptions.RemoveEmptyEntries);
            }

            if (textCommand == null || textCommand.Length == 0)
            {
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