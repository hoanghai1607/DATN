using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProjectManager.Models.Entities
{
    public class TaskUserMember
    {
        public Guid Id { get; set; }
        public Guid? Member { get; set; }
        public Guid? TaskId { get; set; }

        public virtual Task? TaskNavigation { get; set; }
        public virtual User? MemberNavigation { get; set; }
    }
}
