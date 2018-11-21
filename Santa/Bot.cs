using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Santa
{
    public static class Bot
    {
        public static TelegramBotClient Client { get; private set; }

        public static void Start(IConfiguration configuration)
        {
            Client = new TelegramBotClient(configuration["token"]);
            Client.SetWebhookAsync($"https://{configuration["address"]}/api/update").ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Console.WriteLine("Something is wrong with your webhook!");
                    Console.WriteLine(t.Exception.GetBaseException());
                }
                else
                {
                    var x = Client.GetWebhookInfoAsync().Result;
                    Console.WriteLine($"Bot is up and running. Webhook url: {x.Url}");
                }
            });
        }
    }
}