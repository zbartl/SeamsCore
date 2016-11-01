using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeamsCore.Domain
{
    public class Page
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Primary { get; set; }

        public string Secondary { get; set; }

        public string Tertiary { get; set; }

        // Site Navigation
        [Required]
        public bool IsInNavigation { get; set; }
        public int Priority { get; set; }

        // SEO
        public string Title { get; set; }
        public string Redirect { get; set; }

        public string EditRole { get; set; }
        [Required]
        public bool IsUserCreated { get; set; }

        public virtual List<PageSlot> Slots { get; set; }

        //User Created Page Only

        [Required]
        public int Columns { get; set; }

        public int? TemplateId { get; set; }

        [ForeignKey("TemplateId")]
        public virtual PageTemplate Template { get; set; }

    }
}
