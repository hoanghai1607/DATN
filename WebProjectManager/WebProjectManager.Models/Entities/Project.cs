using System;
using System.Collections.Generic;

namespace WebProjectManager.Models.Entities
{
    public partial class Project
    {
        public Project()
        {
            Tabs = new HashSet<Tab>();
            Invites = new HashSet<Invite>();
            MemberProjects = new HashSet<MemberProject>();
            TicketProjects = new HashSet<TicketProject>();
            TimesheetProjects = new HashSet<TimesheetProject>();
        }

        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public string? Name { get; set; }
        public int? NumberMember { get; set; }
        public string? Image { get; set; }
        public int? Order { get; set; }
        public int? Background { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? TimeExpiry { get; set; }
        public bool? IsActive { get; set; }

        public virtual User? CreatedByNavigation { get; set; }
        public virtual ICollection<Tab> Tabs { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }
        public virtual ICollection<MemberProject> MemberProjects { get; set; }
        public virtual ICollection<TicketProject> TicketProjects { get; set; }
        public virtual ICollection<TimesheetProject> TimesheetProjects { get; set; }
    }
}
