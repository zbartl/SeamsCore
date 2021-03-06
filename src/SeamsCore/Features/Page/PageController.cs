﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SeamsCore.Infrastructure.Exceptions;

namespace SeamsCore.Features.Page
{
    public class PageController : Controller
    {
        private readonly IMediator _mediator;

        public PageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Load(Load.Query query)
        {
            var model = await _mediator.Send(query);
            if (string.IsNullOrEmpty(model.TemplateView))
            {
                return View(model);
            }

            return View(model.TemplateView, model);
        }

        [Route("page/save")]
        [HttpPost]
        public async Task<JsonResult> Save([FromBody] Save.Command command)
        {
            try
            {
                await _mediator.Send(command);
            }
            catch (CommandException e)
            {
                var failed = new { Success = "False", Message = e.Message };
                return Json(failed);
            }

            var result = new { Success = "True", Message = "Error Message" };
            return Json(result);
        }

        [Route("page/list")]
        public async Task<IActionResult> List()
        {
            var pages = await _mediator.Send(new List.Query());
            return View(pages);
        }

        [Route("page/create/{primary?}/{secondary?}/{tertiary?}")]
        public async Task<IActionResult> Create(string primary = "", string secondary = "", string tertiary = "")
        {
            var command = await _mediator.Send(new Create.Query { Primary = primary, Secondary = secondary, Tertiary = tertiary });
            return View(command);
        }

        [HttpPost]
        [Route("page/create/{primary?}/{secondary?}/{tertiary?}")]
        public async Task<IActionResult> Create(Create.Command command)
        {
            await _mediator.Send(command);

            return RedirectToAction("List");
        }

        [HttpPost]
        [Route("page/update-priority")]
        public async Task<IActionResult> UpdatePriority([FromBody] UpdatePriority.Command command)
        {
            await _mediator.Send(command);

            return Json("success");
        }
    }
}
