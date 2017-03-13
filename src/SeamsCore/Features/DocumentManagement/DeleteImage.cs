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
    public class DeleteImage
    {
        public class Command : IRequest<Unit>
        {
            public string ImagesDirectory { get; set; }
            public string SubDirectory { get; set; }
            public string ImageName { get; set; }
        }

        public class CommandValidator: AbstractValidator<Command>
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
                var directory = message.ImagesDirectory + (string.IsNullOrEmpty(message.SubDirectory) ? "" : message.SubDirectory + "/");
                var path = directory + message.ImageName;

                File.SetAttributes(path, FileAttributes.Normal);
                File.Delete(path);

                return Unit.Value;
            }
        }
    }
}
