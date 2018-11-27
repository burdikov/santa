using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Santa.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Up()
        {
            return Json(new {status = "up and running"});
        }
    }
}
