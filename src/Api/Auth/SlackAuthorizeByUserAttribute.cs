using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Api;
using Models.Config;
using Newtonsoft.Json;

namespace Api.Auth
{
    public class SlackAuthorizeByUserAttribute : ActionFilterAttribute
    {
        private readonly string[] _authorizedUsernames;
        private readonly string[] _authorizedChannels;

        public SlackAuthorizeByUserAttribute(string[] authorizedUsernames = null, string[] authorizedChannels = null)
        {
            _authorizedUsernames = authorizedUsernames;
            _authorizedChannels = authorizedChannels;
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var username = context.HttpContext.Request.Form["user_name"].FirstOrDefault();
            var userId = context.HttpContext.Request.Form["user_id"].FirstOrDefault();
            var channelName = context.HttpContext.Request.Form["channel_name"].FirstOrDefault();
            
            if (_authorizedUsernames.Contains(username) || _authorizedChannels.Contains(channelName))
            {
                base.OnActionExecuting(context);
                return;
            }

            context.Result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(new SlackResponse
                {
                    ResponseType = SlackResponseType.Ephemeral,
                    Text = $"You are not authorized to do that action <@{userId}> [{username}]."
                }),
                StatusCode = 200
            };
        }
    }
}