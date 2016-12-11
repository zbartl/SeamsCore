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
    public class Load
    {
        public class Query : IAsyncRequest<Result>
        {
            public string Primary { get; set; }
            public string Secondary { get; set; }
            public string Tertiary { get; set; }
        }

        public class Result
        {
            public string Primary { get; set; }
            public string Secondary { get; set; }
            public string Tertiary { get; set; }
            public string Title { get; set; }
            public int Columns { get; set; }
            public List<Slot> Slots { get; set; } = new List<Slot>();
        }

        public class Slot
        {
            public string SeaId { get; set; }
            public int PageColumn { get; set; }
            public string Html { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Query, Result>
        {
            private readonly SeamsContext _db;

            public Handler(SeamsContext db)
            {
                _db = db;
            }

            public async Task<Result> Handle(Query message)
            {
                var page = await _db.Pages
                    .Include(p => p.Slots)
                    .ThenInclude(s => s.Versions)
                    .FirstOrDefaultAsync(p =>
                        p.Primary == message.Primary &&
                        p.Secondary == message.Secondary &&
                        p.Tertiary == message.Tertiary);

                return Mapper.Map<Result>(page);
            }
        }
    }
}
