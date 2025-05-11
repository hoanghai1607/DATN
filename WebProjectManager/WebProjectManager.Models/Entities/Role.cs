using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace WebProjectManager.Models.Entities
{
    public partial class Role : IdentityRole<Guid>
    {
        public string? Description { get; set; }
        public bool? IsActive { get; set; }

    }
}
