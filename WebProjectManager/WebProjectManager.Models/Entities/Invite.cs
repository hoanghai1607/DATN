using System;
using System.Collections.Generic;

namespace WebProjectManager.Models.Entities
{
    public partial class Invite
    {
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ProjectId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? TimeExpiry { get; set; }
        public bool? IsActive { get; set; }

        public virtual User? CreatedByNavigation { get; set; }
        public virtual Project? Project { get; set; }
    }
}
