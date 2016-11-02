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

    public class Save
    {
        public class Command : IRequest
        {
            public Command()
            {
                ModifiedSlots = new List<ModifiedSlot>();
            }

            public List<ModifiedSlot> ModifiedSlots { get; private set; }

            public class ModifiedSlot
            {
                public string Primary { get; set; }
                public string Secondary { get; set; }
                public string Tertiary { get; set; }
                public string SeaId { get; set; }
                public string Html { get; set; }
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.ModifiedSlots).NotEmpty().Must(m => !m.Any(s => string.IsNullOrEmpty(s.Primary)));
            }
        }

        public class Handler : RequestHandler<Command>
        {
            private readonly SeamsContext _db;

            public Handler(SeamsContext db)
            {
                _db = db;
            }

            protected override void HandleCore(Command message)
            {
                var page = _db.Pages.FirstOrDefault(p =>
                    p.Primary == message.ModifiedSlots.First().Primary &&
                    p.Secondary == message.ModifiedSlots.First().Secondary &&
                    p.Tertiary == message.ModifiedSlots.First().Tertiary);

                foreach (var slot in message.ModifiedSlots)
                {
                    UpdatePageSlot(page, slot.SeaId, slot.Html);
                }
            }

            private void UpdatePageSlot(Page page, string seaId, string html)
            {
                var slot = _db.PageSlots.FirstOrDefault(s => s.PageId == page.Id && s.SeaId == seaId);

                if (slot == null)
                {
                    slot = new PageSlot
                    {
                        SeaId = seaId,
                        PageColumn = 0,
                        Versions = new List<PageSlotHtml>()
                        {
                            new PageSlotHtml()
                            {
                                Html = html
                            }
                        }
                    };
                    page.Slots.Add(slot);
                }
                else
                {
                    slot.Versions.Add(new PageSlotHtml
                    {
                        Html = html
                    });
                }
            }
        }
    }
}
