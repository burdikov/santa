using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Santa.Controllers
{
    public class ApiController : Controller
    {
        [HttpPost]
        public IActionResult Update([FromBody] Telegram.Bot.Types.Update u)
        {
            Bot.Client.SendTextMessageAsync(u.Message.Chat.Id, "hi");

            return Ok();
        }
    }
}
