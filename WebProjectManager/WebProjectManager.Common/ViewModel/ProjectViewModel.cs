using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProjectManager.Common.ViewModel
{
    public class ProjectViewModel
    {
        public string Name { get; set; }
        public int NumberMember { get; set; }
        public int Background { get; set; }
        public DateTime TimeExpiry { get; set; }
        public bool IsActive { get; set; }
    }
}
