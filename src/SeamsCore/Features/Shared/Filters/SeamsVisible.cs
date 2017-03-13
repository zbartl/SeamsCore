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
            var primary = context.ActionDescriptor.RouteValues["controller"];
            var secondary = context.ActionDescriptor.RouteValues["action"];

            //Create a new page if this controller / action does not have one yet.
            await _mediator.Send(new Page.CreateWhenNonexistent.Command
            {
                Primary = primary,
                Secondary = secondary,
                Tertiary = ""
            });

            var page = await _mediator.Send(new Page.Load.Query
            {
                Primary = primary,
                Secondary = secondary,
                Tertiary = ""
            });
            //var page = new Page.Load.Result
            //{
            //    Primary = primary,
            //    Secondary = secondary,
            //    Tertiary = "",
            //    Slots = new List<Page.Load.Slot>()
            //};

            var controller = context.Controller as Controller;
            if (controller != null)
            {
                controller.TempData["page"] = page;
            }

            await next();
        }
    }
}
