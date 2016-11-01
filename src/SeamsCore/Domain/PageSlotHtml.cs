using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeamsCore.Domain
{
    public class PageSlotHtml
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Html { get; set; }

        public int SlotId { get; set; }

        [ForeignKey("SlotId")]
        [Required]
        public virtual PageSlot Slot { get; set; }
    }
}
