﻿using Api.Auth;
using Api.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.Config;
using Services;
using Services.Auth;
using Services.BackgroundServices;
using Services.Events.Actions;
using Services.Events.Handlers;
using Services.Events.Matchers;
using Services.Events.Processors;
using Services.Slack;
using Services.Storage;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            services.AddHttpClient<ISlackClient, SlackClient>();
            services.AddSingleton<SlackEventProducer>();
            services.AddSingleton<EventStorage>();
            
            services.AddHostedService<EventListenerBackgroundService>();
            services.AddHostedService<ProcessingConfigurationBackgroundService>();

            services.AddSingleton<ISlackEventHandler, SlackEventHandler>();
            services.AddSingleton<ISlackEventIdentifier, SlackEventIdentifier>();

            services.AddSingleton<IEventProcessorProvider, EventProcessorProvider>();
            services.AddSingleton<IProcessingConfigurationStorage, FileProcessingStorage>();
            
            services.AddSingleton<IEventMatcherFactory, EventMatcherFactory>();
            services.AddSingleton<IActionExecutorFactory, ActionExecutorFactory>();

            //auth
            services.AddSingleton<IAuthConfigurationRepository, AuthConfigurationRepository>();

            services.AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, SlackAuthenticationHandler>(
                    SlackAuthenticationHandler.AuthenticationScheme, 
                    null);

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<SlackCommandMiddleware>();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
