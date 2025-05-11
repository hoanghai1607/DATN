using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebProjectManager.Common.Authentication
{
    public class AuthResult
    {
        public object Token { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
