namespace SeamsCore.Features.Page
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using HtmlTags;
    using HtmlTags.Conventions;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.DependencyInjection;
    using SeamsCore.Domain;
    using System.Linq;
    using Microsoft.AspNetCore.Html;

    public static class HtmlHelperExtensions
    {
        public static HtmlTag EmergePageContent(this IHtmlHelper<Load.Result> helper)
        {
            var model = helper.ViewData.Model;

            var tag = new HtmlTag("div")
                .Attr("data-editable", "page")
                .AddClass("row");

            if (model.Columns == 2)
            {
                var leftColumnSlots = model.Slots.Where(x => x.PageColumn == 1).ToList();
                var rightColumnSlots = model.Slots.Where(x => x.PageColumn == 2).ToList();

                var leftColumn = new HtmlTag("div")
                    .AddClass("col-md-6");

                foreach (var slot in leftColumnSlots)
                {
                    var slotTag = new HtmlTag("div")
                        .Attr("data-editable", "slot")
                        .Attr("data-id", slot.SeaId)
                        .AppendHtml(new HtmlString(slot.Html).ToString());
                    leftColumn.Append(slotTag);
                }

                tag.Append(leftColumn);

                var rightColumn = new HtmlTag("div")
                    .AddClass("col-md-6");

                foreach (var slot in rightColumnSlots)
                {
                    var slotTag = new HtmlTag("div")
                        .Attr("data-editable", "slot")
                        .Attr("data-id", slot.SeaId)
                        .AppendHtml(new HtmlString(slot.Html).ToString());
                    rightColumn.Append(slotTag);
                }

                tag.Append(rightColumn);
            }
            else
            {
                var section = new HtmlTag("div")
                    .AddClass("col-md-12");

                foreach (var slot in model.Slots)
                {
                    var slotTag = new HtmlTag("div")
                        .Attr("data-editable", "slot")
                        .Attr("data-id", slot.SeaId)
                        .AppendHtml(new HtmlString(slot.Html).ToString());
                    section.Append(slotTag);
                }

                tag.Append(section);
            }

            return tag;
        }

        public static HtmlTag EmergePageContent(this IHtmlHelper<Page> helper, string seaid)
        {
            var model = helper.ViewData.Model;

            var slotTag = new HtmlTag("div")
                .Attr("data-editable", "slot")
                .Attr("data-id", seaid);

            var slot = model.Slots.FirstOrDefault(s => s.SeaId == seaid);
            if (slot == null)
            {
                slotTag.Attr("data-placeholder", "1");
                slotTag.Append(new HtmlTag("p").AppendHtml("Click to enter text"));
            }
            else
            {
                slotTag.AppendHtml(new HtmlString(slot.LatestVersion.Html).ToString());
            }

            return slotTag;
        }
    }
}
