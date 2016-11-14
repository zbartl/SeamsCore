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
        private readonly UserManager<User> _userManager;

        public PageSettingsController(IMediator mediator, UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [Route("load/{Primary}/{Secondary?}/{Tertiary?}")]
        public async Task<IActionResult> Load(Load.Query query)
        {
            var settings = await _mediator.SendAsync(query);
            return View(settings);
        }

        [Route("save")]
        [HttpPost]
        public async Task<JsonResult> Save(Save.Command command)
        {
            await _mediator.SendAsync(command);

            var result = new { Success = "True", Message = "Error Message" };
            return Json(result);
        }

    }
}
