using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeamsCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace SeamsCore.Features.Home
{
    public class HomeController : Controller
    {
        private readonly SeamsContext _db;

        public HomeController(SeamsContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var templates = await _db.PageTemplates.ToListAsync();
            ViewBag.Templates = templates;
            ViewBag.Message = "Testing 1 2 4";
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
