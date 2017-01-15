using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeamsCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SeamsCore.Features.Shared.Filters;
using MediatR;

namespace SeamsCore.Features.Home
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("~/")]
        [ServiceFilter(typeof(SeamsVisible))]
        public async Task<IActionResult> Index()
        {
            var page = TempData["page"] as Page.Load.Result;
            return View(page);
        }

        [Route("home/test")]
        public async Task<IActionResult> Test()
        {
            var data = await _mediator.SendAsync(new Test.Query { Divisor = 0 });
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
