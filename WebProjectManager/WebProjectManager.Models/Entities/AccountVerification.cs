using System;
using System.Collections.Generic;

namespace WebProjectManager.Models.Entities
{
    public partial class AccountVerification
    {
        public Guid Id { get; set; }
        public Guid? IdUser { get; set; }
        public bool? VerificationWith { get; set; }
        public string? Code { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? ExpiryOn { get; set; }

        public virtual User? IdUserNavigation { get; set; }
    }
}
