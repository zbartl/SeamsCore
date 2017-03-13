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
    public class CreateDirectory
    {
        public class Command : IRequest<Unit>
        {
            public string WorkingDirectory { get; set; }
            public string SubDirectory { get; set; }
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
                var dirPath = message.WorkingDirectory + message.SubDirectory;

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                return Unit.Value;
            }
        }
    }
}
