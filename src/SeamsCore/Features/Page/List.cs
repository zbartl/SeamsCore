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
    /// Defines the Query, Result and Handler for Retrieving an organized list of Pages.
    /// </summary>
    public class List
    {
        public class Query : IAsyncRequest<Result>
        {
        }

        public class Result
        {
            public List<PrimaryPage> PrimaryPages { get; set; } = new List<PrimaryPage>();
        }
        
        public class PrimaryPage : AbbreviatedPage
        {
            public List<SecondaryPage> SecondaryPages { get; set; } = new List<SecondaryPage>();
        }
        public class SecondaryPage : AbbreviatedPage
        {
            public List<TertiaryPage> TertiaryPages { get; set; } = new List<TertiaryPage>();
        }
        public class TertiaryPage : AbbreviatedPage
        {
        }

        public abstract class AbbreviatedPage
        {
            public int Id { get; set; }
            public string Primary { get; set; }
            public string Secondary { get; set; }
            public string Tertiary { get; set; }
            public int Priority { get; set; }
            public bool IsUserCreated { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Query, Result>
        {
            private readonly SeamsContext _db;

            public Handler(SeamsContext db)
            {
                _db = db;
            }

            /// <summary>
            /// Retrieves all pages from database and organizes them into ordered, nested lists of primary, secondary and tertiary pages.
            /// </summary>
            /// <param name="message">The query.</param>
            /// <returns>A task whose result contains the ordered, nested list of pages.</returns>
            public async Task<Result> Handle(Query message)
            {
                var pages = await _db.Pages.ToListAsync();

                var primaryPages = pages
                        .Where(p => !string.IsNullOrWhiteSpace(p.Primary))
                        .Where(p => string.IsNullOrEmpty(p.Secondary) ||
                                    (!p.IsUserCreated && (p.Secondary == "Index" || p.Secondary == p.Primary)))
                        .OrderBy(p => p.Priority);

                var result = new Result();

                foreach (var primary in primaryPages)
                {
                    var abbreviatedPrimaryPage = Mapper.Map<PrimaryPage>(primary);

                    var secondaryPages = pages
                        .Where(p => p.Primary == primary.Primary)
                        .Where(p => !string.IsNullOrWhiteSpace(p.Secondary))
                        .Where(p => p.Secondary != "Index" && p.Secondary != p.Primary)
                        .Where(p => string.IsNullOrWhiteSpace(p.Tertiary))
                        .OrderBy(p => p.Priority);

                    foreach (var secondary in secondaryPages)
                    {
                        var abbreviatedSecondaryPage = Mapper.Map<SecondaryPage>(secondary);
                        abbreviatedPrimaryPage.SecondaryPages.Add(abbreviatedSecondaryPage);

                        var tertiaryPages = pages
                            .Where(p => p.Primary == primary.Primary)
                            .Where(p => p.Secondary == secondary.Secondary)
                            .Where(p => !string.IsNullOrWhiteSpace(p.Tertiary))
                            .OrderBy(p => p.Priority);

                        foreach (var tertiary in tertiaryPages)
                        {
                            var abbreviatedTertiaryPage = Mapper.Map<TertiaryPage>(tertiary);
                            abbreviatedSecondaryPage.TertiaryPages.Add(abbreviatedTertiaryPage);
                        }
                    }

                    result.PrimaryPages.Add(abbreviatedPrimaryPage);
                }

                return result;
            }
        }
    }
}
