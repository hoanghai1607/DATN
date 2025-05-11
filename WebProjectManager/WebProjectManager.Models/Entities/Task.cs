using System;
using System.Collections.Generic;

namespace WebProjectManager.Models.Entities
{
    public partial class Task
    {
        public Task()
        {
            TaskUserMember = new HashSet<TaskUserMember>();
        }
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? CardId { get; set; }
        public string? Name { get; set; }
        public int? NumberMember { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? TimeExpiry { get; set; }
        public string? Comment { get; set; }
        public string? Icon { get; set; }
        public int? Order { get; set; }
        public string? Type { get; set; }
        public bool? IsActive { get; set; }

        public virtual Card? Card { get; set; }
        public virtual User? CreatedByNavigation { get; set; }
        public virtual ICollection<TaskUserMember> TaskUserMember { get; set; }
    }
}
