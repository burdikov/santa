using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Santa.Controllers
{
    public class TestController : Controller
    {
        SantaDbContext context;

        public TestController(SantaDbContext context)
        {
            this.context = context;
        }

        public IActionResult Up()
        {
            var rowsAffected = context.Database.ExecuteSqlCommand("select 1");

            return Json(new { status = "up and running", rowsAffected });
        }
    }
}
