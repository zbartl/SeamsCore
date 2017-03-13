using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SeamsCore.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace SeamsCore.Features.PageSettings
{
    [Authorize(Policy = "Edit")]
    [Route("page-settings")]
    public class PageSettingsController : Controller
    {
        private readonly IMediator _mediator;

        public PageSettingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("save")]
        [HttpPost]
        public async Task<JsonResult> Save(Save.Command command)
        {
            await _mediator.Send(command);

            var result = new { Success = "True", Message = "Error Message" };
            return Json(result);
        }

    }
}
