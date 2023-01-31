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


        internal string Order(List<Produkt> items)
        {
            //create new lists of items
            List<Produkt> itemsOrdered = new List<Produkt>();
            //loop through items and add to items ordered
            foreach (var item in items)
            {
                //check if item is already in list
                var itemInList = itemsOrdered.FirstOrDefault(x => x.Id == item.Id);
                if (itemInList != null)
                {
                    //if item is in list, increase quantity
                    itemInList.Quantity += item.Quantity;
                }
                else
                {
                    //if item is not in list, add to list
                    itemsOrdered.Add(item);
                }
                
                
            }
            
            return JsonSerializer.Serialize<List<Produkt>>(itemsOrdered);


        }
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
