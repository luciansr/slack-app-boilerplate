using Api.Middleware;
using Clients.Slack;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.Config;
using Services;
using Services.BackgroundServices;
using Services.Events.Handlers;
using Services.Events.Processors;
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

            services.AddHttpClient<SlackClient>();
            services.AddSingleton<SlackEventProducer>();
            services.AddSingleton<EventStorage>();
            
            services.AddHostedService<EventListenerBackgroundService>();

            services.AddSingleton<SlackEventHandler>();
            services.AddSingleton<ChannelMessageEventHandler>();
            services.AddSingleton<ThreadMessageEventHandler>();
            services.AddSingleton<UserJoinedEventHandler>();
            services.AddSingleton<AppMentionEventHandler>();
            services.AddSingleton<ReactionAddedEventHandler>();
            services.AddSingleton<IEventProcessorProvider, EventProcessorProvider>();
            

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
            app.UseMiddleware<SlackMiddleware>();
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
