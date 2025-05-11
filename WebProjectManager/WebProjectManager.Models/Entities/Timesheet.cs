using System;
using System.Collections.Generic;

namespace WebProjectManager.Models.Entities
{
    public partial class Timesheet
    {
        public Timesheet()
        {
            TimesheetProjects = new HashSet<TimesheetProject>();
        }

        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? TimeBegin { get; set; }
        public DateTime? TimeExpiry { get; set; }
        public string? Icon { get; set; }
        public bool? IsActive { get; set; }

        public virtual User? CreatedByNavigation { get; set; }
        public virtual ICollection<TimesheetProject> TimesheetProjects { get; set; }
    }
}
