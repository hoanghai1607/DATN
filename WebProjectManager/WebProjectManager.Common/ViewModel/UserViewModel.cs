using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProjectManager.Common.ViewModel
{
    public class UserViewModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
