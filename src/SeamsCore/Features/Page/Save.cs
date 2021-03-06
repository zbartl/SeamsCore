﻿namespace SeamsCore.Features.Page
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

    /// <summary>
    /// Defines the Command, Validation and Handler for saving a user edited Page's html content.
    /// </summary>
    public class Save
    {
        public class Command : IRequest
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

        public class Handler : IAsyncRequestHandler<Command>
        {
            private readonly SeamsContext _db;

            public Handler(SeamsContext db)
            {
                _db = db;
            }

            /// <summary>
            /// Retrieves the edited Page from the database, adds new Slots and updates html content.
            /// </summary>
            /// <param name="message">The command.</param>
            /// <returns>A task.</returns>
            public async Task Handle(Command message)
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
