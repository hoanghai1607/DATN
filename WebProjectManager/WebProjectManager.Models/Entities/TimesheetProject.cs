using System;
using System.Collections.Generic;

namespace WebProjectManager.Models.Entities
{
    public partial class TimesheetProject
    {
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? TimesheetId { get; set; }
        public bool? IsActive { get; set; }

        public virtual User? CreatedByNavigation { get; set; }
        public virtual Project? Project { get; set; }
        public virtual Timesheet? Timesheet { get; set; }
    }
}
