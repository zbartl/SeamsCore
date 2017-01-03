namespace SeamsCore.Features.Page
{
    using AutoMapper;
    using MediatR;
    using SeamsCore.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SeamsCore.Domain;
    using FluentValidation;

    /// <summary>
    /// Defines the Command, Validation and Handler for saving a user edited Page's html content.
    /// </summary>
    public class UpdatePriority
    {
        public class Command : IAsyncRequest<Unit>
        {
            public List<int> Ids { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.Ids).NotEmpty();
            }
        }

        public class Handler : IAsyncRequestHandler<Command, Unit>
        {
            private readonly SeamsContext _db;

            public Handler(SeamsContext db)
            {
                _db = db;
            }

            public async Task<Unit> Handle(Command message)
            {
                var i = 0;
                foreach (var id in message.Ids)
                {
                    var page = await _db.Pages.FirstOrDefaultAsync(p => p.Id == id);
                    page.Priority = i;
                    i++;
                }

                return Unit.Value;
            }
        }
    }
}
