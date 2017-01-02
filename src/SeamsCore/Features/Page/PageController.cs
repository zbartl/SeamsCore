﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SeamsCore.Infrastructure.Exceptions;

namespace SeamsCore.Features.Page
{
    [Route("page")]
    public class PageController : Controller
    {
        private readonly IMediator _mediator;

        public PageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("load/{Primary}/{Secondary?}/{Tertiary?}")]
        public async Task<IActionResult> Load(Load.Query query)
        {
            var model = await _mediator.SendAsync(query);
            return View(model);
        }

        [Route("save")]
        [HttpPost]
        public async Task<JsonResult> Save([FromBody] Save.Command command)
        {
            try
            {
                await _mediator.SendAsync(command);
            }
            catch (CommandException e)
            {
                var failed = new { Success = "False", Message = e.Message };
                return Json(failed);
            }

            var result = new { Success = "True", Message = "Error Message" };
            return Json(result);
        }

        [Route("list")]
        public async Task<IActionResult> List()
        {
            var pages = await _mediator.SendAsync(new List.Query());
            return View(pages);
        }

        [Route("create/{primary?}/{secondary?}/{tertiary?}")]
        public async Task<IActionResult> Create(string primary = "", string secondary = "", string tertiary = "")
        {
            var templates = await _mediator.SendAsync(new Create.Query { Primary = primary, Secondary = secondary, Tertiary = tertiary });
            return View(templates);
        }

        [HttpPost]
        [Route("create/{primary?}/{secondary?}/{tertiary?}")]
        public async Task<IActionResult> Create(Create.Command command)
        {
            await _mediator.SendAsync(command);

            return RedirectToAction("List");
        }
    }
}
