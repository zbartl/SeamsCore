using AutoMapper;
using MediatR;
using SeamsCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SeamsCore.Domain;
using Microsoft.AspNetCore.Identity;

namespace SeamsCore.Features.PageSettings
{
    public class Save
    {
        public class Command : IAsyncRequest
        {
            public string Primary { get; set; }
            public string Secondary { get; set; }
            public string Tertiary { get; set; }
            public string Title { get; set; }
            public string Redirect { get; set; }
            public bool IsInNavigation { get; set; }
            public int Priority { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly SeamsContext _db;

            public Handler(SeamsContext db)
            {
                _db = db;
            }

            protected override async Task HandleCore(Command message)
            {
                var page = await _db.Pages.FirstOrDefaultAsync(p =>
                    p.Primary == message.Primary &&
                    p.Secondary == message.Secondary &&
                    p.Tertiary == message.Tertiary);

                if (page == null)
                {
                    return;
                }

                page.Handle(message);
            }
        }
    }
}
