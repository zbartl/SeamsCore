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
        public class Query : IAsyncRequest<Command>
        {
            public string Primary { get; set; }
            public string Secondary { get; set; }
            public string Tertiary { get; set; }
        }

        public class Command : IAsyncRequest<Unit>
        {
            public string Primary { get; set; }
            public string Secondary { get; set; }
            public string Tertiary { get; set; }
            public List<Template> AvailableTemplates { get; set; } = new List<Template>();
            public int TemplateId { get; set; }
        }

        public class Template
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string View { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, Command>
        {
            private readonly SeamsContext _db;

            public QueryHandler(SeamsContext db)
            {
                _db = db;
            }

            public async Task<Command> Handle(Query message)
            {
                var templates = await _db.PageTemplates.ToListAsync();
                var result = new Command();
                result.Primary = message.Primary;
                result.Secondary = message.Secondary;
                result.Tertiary = message.Tertiary;
                result.AvailableTemplates = Mapper.Map<List<Template>>(templates);
                return result;
            }
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
                var nextPriority = 0;
                if (!string.IsNullOrEmpty(message.Tertiary))
                {
                    nextPriority = await _db.Pages.CountAsync(p => p.Primary == message.Primary && p.Secondary == message.Secondary) - 1;
                }
                else if (!string.IsNullOrEmpty(message.Secondary))
                {
                    nextPriority = await _db.Pages.CountAsync(p => p.Primary == message.Primary) - 1;
                }
                else
                {
                    nextPriority = await _db.Pages.CountAsync() - 1;
                }

                _db.Pages.Add(new Page
                {
                    Primary = message.Primary,
                    Secondary = message.Secondary,
                    Tertiary = message.Tertiary,
                    Priority = nextPriority,
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
