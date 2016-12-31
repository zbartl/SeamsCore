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
        public class Command : IAsyncRequest
        {
            public List<ModifiedSlot> ModifiedSlots { get; set; }

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

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly SeamsContext _db;

            public Handler(SeamsContext db)
            {
                _db = db;
            }

            protected override async Task HandleCore(Command message)
            {
                var page = await _db.Pages
                        .Include(p => p.Slots)
                        .FirstOrDefaultAsync(p =>
                            p.Primary == message.ModifiedSlots.First().Primary &&
                            p.Secondary == message.ModifiedSlots.First().Secondary &&
                            p.Tertiary == message.ModifiedSlots.First().Tertiary);

                PageSlot slot;

                foreach (var modifiedSlot in message.ModifiedSlots)
                {
                    slot = await _db.PageSlots
                        .Include(s => s.Versions)
                        .FirstOrDefaultAsync(s => s.PageId == page.Id && s.SeaId == modifiedSlot.SeaId);
                    if (slot == null)
                    {
                        slot = new PageSlot();
                        slot.SeaId = modifiedSlot.SeaId;
                        slot.Versions = new List<PageSlotHtml>();
                        page.Slots.Add(slot);
                    }
                    slot.UpdateHtml(modifiedSlot.Html);
                }
            }
        }
    }
}
