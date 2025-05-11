using System;
using System.Collections.Generic;

namespace WebProjectManager.Models.Entities
{
    public partial class TicketProject
    {
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? TicketId { get; set; }

        public virtual User? CreatedByNavigation { get; set; }
        public virtual Project? Project { get; set; }
        public virtual Ticket? Ticket { get; set; }
    }
}
