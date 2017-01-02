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
    using SeamsCore.Domain;

    /// <summary>
    /// Defines the Query, Result and Handler for Retrieving the list of Page Templates.
    /// Also Defines the Command, Validator and Handler for creation of a new user defined Page.
    /// </summary>
    public class Create
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
            public List<Template> Templates { get; set; } = new List<Template>();
        }

        public class Template
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string View { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, Result>
        {
            private readonly SeamsContext _db;

            public QueryHandler(SeamsContext db)
            {
                _db = db;
            }

            public async Task<Result> Handle(Query message)
            {
                var templates = await _db.PageTemplates.ToListAsync();
                var result = new Result();
                result.Primary = message.Primary;
                result.Secondary = message.Secondary;
                result.Tertiary = message.Tertiary;
                result.Templates = Mapper.Map<List<Template>>(templates);
                return result;
            }
        }

        public class Command : IAsyncRequest<Unit>
        {
            public string Primary { get; set; }
            public string Secondary { get; set; }
            public string Tertiary { get; set; }
            public int TemplateId { get; set; }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, Unit>
        {
            private readonly SeamsContext _db;

            public CommandHandler(SeamsContext db)
            {
                _db = db;
            }

            public async Task<Unit> Handle(Command message)
            {
                _db.Pages.Add(new Page
                {
                    Primary = message.Primary,
                    Secondary = message.Secondary,
                    Tertiary = message.Tertiary,
                    Columns = 0,
                    TemplateId = message.TemplateId,
                    IsInNavigation = false,
                    IsUserCreated = true
                });

                return Unit.Value;
            }
        }
    }
}
