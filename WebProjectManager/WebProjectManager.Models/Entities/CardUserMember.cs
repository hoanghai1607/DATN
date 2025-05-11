using System;
using System.Collections.Generic;

namespace WebProjectManager.Models.Entities
{
    public partial class CardUserMember
    {
        public Guid Id { get; set; }
        public Guid? Member { get; set; }
        public Guid? CardId { get; set; }

        public virtual Card? Card { get; set; }
        public virtual User? MemberNavigation { get; set; }
    }
}
