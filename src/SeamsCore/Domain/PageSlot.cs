using System;
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
            get { return Versions.Last(); }
        }
    }
}
