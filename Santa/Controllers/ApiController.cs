using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Types.Enums;

namespace Santa.Controllers
{
    public class ApiController : Controller
    {
        IMemoryCache cache;

        public ApiController(IMemoryCache cache)
        {
            this.cache = cache;
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Telegram.Bot.Types.Update u)
        {
            // We only accept not empty text messages from non-bot users.
            var msg = u.Message;

            if (msg != null && !string.IsNullOrWhiteSpace(msg.Text) && !msg.From.IsBot)
            {
                var beh = cache.GetOrCreate<Behaviour>(msg.From.Id, entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromHours(1);
                    return new IdleBehaviour(Bot.Client);
                });

                // One message from user at a time.
                var sem = new Semaphore(1, 1, msg.From.Id.ToString());
                if (sem.WaitOne(0))
                {
                    try
                    {
                        var next = await beh.ProcessAsync(u);
                        cache.Set(msg.From.Id, next ?? new IdleBehaviour(Bot.Client));
                    }
                    finally
                    {
                        sem.Release();
                    }
                }
            }

            return Ok();
        }
    }
}
