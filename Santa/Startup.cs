using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Santa
{
    public class Startup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            builder.AddJsonFile("secrets.json");
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddDbContext<SantaDbContext>(options =>
            {
                options.UseSqlServer(Configuration["connstr"]);
            });

            services.AddSingleton(StartBot());
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService <SantaDbContext>();
                context.Database.Migrate();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Default",
                    template: "/{controller=Test}/{action=Up}"
                );
            });
        }

        private TelegramBotClient StartBot()
        {
            var client = new TelegramBotClient(Configuration["token"]);
            client.SetWebhookAsync($"https://{Configuration["address"]}/api/update").ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Console.WriteLine("Something is wrong with your webhook!");
                    Console.WriteLine(t.Exception?.GetBaseException());
                }
                else
                {
                    var x = client.GetWebhookInfoAsync().Result;
                    Console.WriteLine($"Bot is up and running. Webhook url: {x.Url}");
                }
            });

            return client;
        }
    }
}
