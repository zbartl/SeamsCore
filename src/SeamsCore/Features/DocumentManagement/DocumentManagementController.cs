using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SeamsCore.Infrastructure.Exceptions;
using SeamsCore.Infrastructure;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;

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
            var result = await _mediator.Send(query);

            return View(result);
        }

        [HttpPost]
        [Route("documents/images/upload/{subDirectory?}")]
        public async Task<IActionResult> UploadImage(string subDirectory, IFormFile image)
        {
            var command = new UploadImage.Command
            {
                AllowedExtensions = new List<string>(_settings.AllowedImageExtensions.Split(',')),
                ImagesDirectory = _environment.WebRootPath + "/uploads/images/",
                SubDirectory = subDirectory,
                Image = image
            };

            try
            {
                await _mediator.Send(command);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json(e.Message);
            }

            return Json("success");
        }

        [HttpPost]
        [Route("documents/images/create-directory")]
        public async Task<IActionResult> CreateDirectory([FromForm] string subDirectory)
        {
            var command = new CreateDirectory.Command
            {
                WorkingDirectory = _environment.WebRootPath + "/uploads/images/",
                SubDirectory = subDirectory
            };

            try
            {
                await _mediator.Send(command);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json(e.Message);
            }

            return Json("success");
        }

        [HttpPost]
        [Route("documents/images/delete")]
        public async Task<IActionResult> DeleteImage([FromForm] string subDirectory, [FromForm] string imageName)
        {
            var command = new DeleteImage.Command
            {
                ImagesDirectory = _environment.WebRootPath + "/uploads/images/",
                SubDirectory = subDirectory,
                ImageName = imageName
            };

            try
            {
                await _mediator.Send(command);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json(e.Message);
            }

            return Json("success");
        }
    }
}
