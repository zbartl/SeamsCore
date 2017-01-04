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
    /// <summary>
    /// Defines the Query, Result and Handler for loading an individual Page and all of its content (Slots).
    /// </summary>
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
            public string TemplateView { get; set; }
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

            /// <summary>
            /// Retrives the specified Page and it's html content from the database for display and CMS purposes.
            /// </summary>
            /// <param name="message">The query.</param>
            /// <returns>A Page and all of its content as Slots.</returns>
            public async Task<Result> Handle(Query message)
            {
                var page = await _db.Pages
                    .Include(p => p.Template)
                    .Include(p => p.Slots)
                    .ThenInclude(p => p.Versions)
                    .FirstOrDefaultAsync(p =>
                        p.Primary == message.Primary &&
                        p.Secondary == message.Secondary &&
                        p.Tertiary == message.Tertiary);

                var result = Mapper.Map<Result>(page);
                return result;
            }
        }
    }
}
