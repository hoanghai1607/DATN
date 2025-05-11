using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProjectManager.Common.ViewModel
{
    public class RegisterViewModel
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? Gender { get; set; }
        public string? Password { get; set; }
    }
}
