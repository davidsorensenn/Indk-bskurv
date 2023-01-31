using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;


namespace Indkøbskurv
{
    internal class Produkt
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }

        
    }
}
