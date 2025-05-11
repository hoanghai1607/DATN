using System;
using System.Collections.Generic;

namespace WebProjectManager.Models.Entities
{
    public partial class MemberProject
    {
        public Guid Id { get; set; }
        public Guid? IdUser { get; set; }
        public Guid? ProjectId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? TimeExpiry { get; set; }
        public bool? IsActive { get; set; }

        public virtual User? IdUserNavigation { get; set; }
        public virtual Project? Project { get; set; }
    }
}
