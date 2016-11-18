using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeamsCore.Features.Shared.Filters
{
    public class SeamsVisible : IAsyncActionFilter
    {
        private readonly IMediator _mediator;

        public SeamsVisible(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Create a new page if this controller/action does not have one yet.
            await _mediator.SendAsync(new Page.CreateWhenNonexistent.Command
            {
                Primary = context.Controller.ToString(),
                Secondary = context.ActionDescriptor.ToString(),
                Tertiary = ""
            });

            var page = await _mediator.SendAsync(new Page.Load.Query
            {
                Primary = context.Controller.ToString(),
                Secondary = context.ActionDescriptor.ToString(),
                Tertiary = ""
            });

            var controller = context.Controller as Controller;
            if (controller != null)
            {
                controller.TempData["page"] = page;
            }

            await next();
        }
    }
}
