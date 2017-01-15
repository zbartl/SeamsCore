using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeamsCore.Features.PageSettings
{
    public class PageSettingsViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public PageSettingsViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync(Load.Query query)
        {
            var model = await _mediator.SendAsync(query);
            return View(model);
        }
    }
}
