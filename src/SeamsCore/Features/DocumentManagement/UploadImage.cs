using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SeamsCore.Features.DocumentManagement
{
    public class UploadImage
    {
        public class Command : IRequest<Unit>
        {
            public List<string> AllowedExtensions { get; set; }
            public string ImagesDirectory { get; set; }
            public string SubDirectory { get; set; }
            public IFormFile Image { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.SubDirectory).Matches("^[A-Za-z0-9]+$");
            }
        }

        public class Handler : IAsyncRequestHandler<Command, Unit>
        {
            public async Task<Unit> Handle(Command message)
            {
                ValidateImageType(message);

                var directory = message.ImagesDirectory + (string.IsNullOrEmpty(message.SubDirectory) ? "" : message.SubDirectory + "/");
                var path = directory + message.Image.FileName;

                if (message.Image.Length > 0)
                {
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await message.Image.CopyToAsync(stream);
                    }
                }

                return Unit.Value;
            }

            private void ValidateImageType(Command message)
            {
                if (!message.AllowedExtensions.Contains(Path.GetExtension(message.Image.FileName)))
                {
                    throw new FluentValidation.ValidationException(new List<ValidationFailure> { new ValidationFailure("Image", $"Files of type {Path.GetExtension(message.Image.FileName)} are not supported.") });
                }
            }
        }
    }
}
