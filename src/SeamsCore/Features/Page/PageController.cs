﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;

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

        [Route("/load/{Primary}/{Secondary?}/{Tertiary?}")]
        public async Task<IActionResult> Load(Load.Query query)
        {
            var model = await _mediator.SendAsync(query);
            return View(model);
        }

        [Route("save")]
        [HttpPost]
        public JsonResult Save(Save.Command command)
        {
            _mediator.Send(command);

            var result = new { Success = "True", Message = "Error Message" };
            return Json(result);
        }

    }
}
