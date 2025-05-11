using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProjectManager.Models.Entities
{
    public class Tab
    {
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ProjectId { get; set; }
        public string? Name { get; set; }
       // public string? Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? TimeExpiry { get; set; }
        public bool? IsActive { get; set; }
        public int? Order { get; set; }
        public virtual User? CreatedByNavigation { get; set; }
        public virtual Project? Project { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
    }
}
