using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indkøbskurv
{
    public class Kunder
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Indkøbskurv indkøbskurv  { get; set; }
    }
}
