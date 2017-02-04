using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SeamsCore.Infrastructure.Exceptions;
using SeamsCore.Infrastructure;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

namespace SeamsCore.Features.DocumentManagement
{
    public class DocumentManagementController : Controller
    {
        private readonly IMediator _mediator;
        private readonly AppSettings _settings;
        private readonly IHostingEnvironment _environment;

        public DocumentManagementController(IMediator mediator, IOptions<AppSettings> settings, IHostingEnvironment environment)
        {
            _mediator = mediator;
            _settings = settings.Value;
            _environment = environment;
        }

        [Route("documents/images/{subDirectory?}")]
        public async Task<IActionResult> Images(string subDirectory)
        {
            var query = new LoadImages.Query
            {
                AllowedExtensions = new List<string>(_settings.AllowedImageExtensions.Split(',')),
                ImagesDirectory = _environment.WebRootPath + "/uploads/images/",
                SubDirectory = subDirectory
            };
            var result = await _mediator.SendAsync(query);

            return View(result);
        }
    }
}
