using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Indkøbskurv
{
    public class Indkøbskurv
    {
        List<Produkt> produkter { get; set; }


        
        internal void AddtoShoppinglist( Produkt produkt)
        {
            produkter.Add(produkt);
        }
        internal void RemoveFromShoppinglist(int index)
        {
                produkter.RemoveAt(index);
        }


        public Indkøbskurv()
        {
            produkter = new List<Produkt>();
        }
        internal List<Produkt> GetShoppinglist()
        {
            return produkter;
        }
    }
}
