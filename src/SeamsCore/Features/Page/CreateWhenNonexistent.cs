using AutoMapper;
using MediatR;
using SeamsCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SeamsCore.Features.Page
{
    using Domain;

    public class CreateWhenNonexistent
    {
        public class Command : IAsyncRequest<Unit>
        {
            public string Primary { get; set; }
            public string Secondary { get; set; }
            public string Tertiary { get; set; }
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
                var page = await _db.Pages.FirstOrDefaultAsync(p =>
                    p.Primary == message.Primary &&
                    p.Secondary == message.Secondary &&
                    p.Tertiary == message.Tertiary);

                if (page == null)
                {
                    _db.Pages.Add(new Page
                    {
                        Primary = message.Primary,
                        Secondary = message.Secondary,
                        Tertiary = message.Tertiary,
                        IsInNavigation = false,
                        IsUserCreated = false
                    });
                }

                return Unit.Value;
            }
        }
    }
}
