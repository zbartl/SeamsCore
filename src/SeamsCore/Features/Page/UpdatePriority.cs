﻿namespace SeamsCore.Features.Page
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
        public class Command : IRequest<Unit>
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

            /// <summary>
            /// Updates each affected Page with a new priority based on the user altered ordering.
            /// </summary>
            /// <param name="message">The command.</param>
            /// <returns></returns>
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
