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
    /// <summary>
    /// Defines the Query, Result and Handler for retrieving the non-html content of a Page.
    /// </summary>
    public class Load
    {
        public class Query : IRequest<Result>
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
            public string Redirect { get; set; }
            public bool IsInNavigation { get; set; }
            public int Priority { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Query, Result>
        {
            private readonly SeamsContext _db;

            public Handler(SeamsContext db)
            {
                _db = db;
            }

            /// <summary>
            /// Retrieves the non html content settings of a Page, such as Title or Redirect.
            /// </summary>
            /// <param name="message">The query.</param>
            /// <returns>A task with Result defining the non html content settings of a Page.</returns>
            public async Task<Result> Handle(Query message)
            {
                var page = await _db.Pages.FirstOrDefaultAsync(p =>
                    p.Primary == message.Primary &&
                    p.Secondary == message.Secondary &&
                    p.Tertiary == message.Tertiary);

                var settings = Mapper.Map<Result>(page);
                return settings;
            }
        }
    }
}
