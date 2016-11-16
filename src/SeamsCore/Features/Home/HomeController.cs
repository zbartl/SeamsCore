using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeamsCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SeamsCore.Features.Shared.Filters;

namespace SeamsCore.Features.Home
{
    public class HomeController : Controller
    {
        private readonly SeamsContext _db;

        public HomeController(SeamsContext db)
        {
            _db = db;
        }

        [SeamsVisible]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
