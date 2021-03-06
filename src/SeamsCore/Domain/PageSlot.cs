﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeamsCore.Domain
{
    public class PageSlot
    {
        [Key]
        public int Id { get; set; }

        public string SeaId { get; set; }

        public int PageId { get; set; }

        [ForeignKey("PageId")]
        [Required]
        public virtual Page Page { get; set; }

        [Required]
        public int PageColumn { get; set; }

        public virtual List<PageSlotHtml> Versions { get; set; }

        [NotMapped]
        public PageSlotHtml LatestVersion
        {
            get { return Versions?.Last(); }
        }

        [NotMapped]
        public string Html
        {
            get { return LatestVersion != null ? LatestVersion.Html : "";  }
        }

        /// <summary>
        /// Updates the HTML for this Page Slot.
        /// </summary>
        /// <param name="html">The user created HTML string produced by the CMS.</param>
        public void UpdateHtml(string html)
        {
            Versions.Add(new PageSlotHtml
            {
                Html = html
            });
        }
    }
}
