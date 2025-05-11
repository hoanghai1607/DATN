using System;
using System.Collections.Generic;

namespace WebProjectManager.Models.Entities
{
    public partial class Card
    {
        public Card()
        {
            CardUserMembers = new HashSet<CardUserMember>();
            Tasks = new HashSet<Task>();
        }

        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? TabId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? NumberMember { get; set; }
        public int? Order { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? TimeExpiry { get; set; }
        public bool? IsActive { get; set; }

        public virtual User? CreatedByNavigation { get; set; }
        public virtual Tab? Tab { get; set; }
        public virtual ICollection<CardUserMember> CardUserMembers { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
