﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Filters;
using Api.Middleware;
using Clients.Slack;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Config;
using SlackAPI;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
//                options.Filters.Add<SlackFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<SlackCustomClient>();
            services.AddSingleton<SlackConnectedClient>();
            services.AddScoped<SlackScopedClient>();
            services.AddSingleton<SlackClient>();

            BindSectionToConfigObject<SlackConfig>(Configuration, services);
        }
        
        private static void BindSectionToConfigObject<TType>(IConfiguration configuration, IServiceCollection services)
            where TType : class, new()
        {
            var typeConfig = new TType();
            configuration.Bind(typeConfig.GetType().Name, typeConfig);
            services.AddSingleton(typeConfig);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

//            app.UseRouter(router =>
//            {
//                router.MapPost("api/slack", context => {
//                    using (var httpClient = new HttpClient())
//                    {
//                        httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "http://localhost:5001/api/slack2")
//                        {
//
//                        });
//                    }
//
//                    return 0;
//                });
//            });

            app.UseMiddleware<SlackMiddleware>();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
