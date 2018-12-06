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
        private readonly IMemoryCache _cache;
        private readonly IServiceProvider _serviceProvider;

        public ApiController(IMemoryCache cache, IServiceProvider serviceProvider)
        {
            _cache = cache;
            _serviceProvider = serviceProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Telegram.Bot.Types.Update u)
        {
            // We only accept not empty text messages from non-bot users.
            var msg = u.Message;

            if (msg != null && !string.IsNullOrWhiteSpace(msg.Text) && !msg.From.IsBot)
            {
                var beh = _cache.GetOrCreate<Behaviour>(msg.From.Id, entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromHours(1);
                    return new IdleBehaviour();
                });

                // TODO synchronize processing of messages from particular chat
                var next = await beh.ProcessAsync(_serviceProvider, u);
                _cache.Set(msg.From.Id, next ?? new IdleBehaviour());
            }

            return Ok();
        }
    }
}
