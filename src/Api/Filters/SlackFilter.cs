using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;

namespace Api.Filters
{
    internal class SlackFilter : IAsyncActionFilter
    {

        public SlackFilter()
        {
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
//            var formDictionary = context.HttpContext.Request.Form.ToDictionary(item => item.Key, item => item.Value.FirstOrDefault()?.Trim());
//            string data = JObject.FromObject(formDictionary).ToString();
//            context.HttpContext.Request.Body.Write(Encoding.UTF8.GetBytes(data));
        }


        private void AddRouteParameter(ActionExecutingContext context, string key, object value)
        {
            if (context.ActionArguments.ContainsKey(key))
            {
                context.ActionArguments.Remove(key);
            }

            context.RouteData.Values.Add(key, value);
            context.ActionArguments.Add(key, value);
        }

    }
}