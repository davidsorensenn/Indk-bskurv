using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Indkøbskurv
{
    internal class Indkøbskurv
    {
        public List<Produkter> produkter  { get; set; }


        public static string Order(List<Produkter> items)
        {
            //create new lists of items
            List<Produkter> itemsOrdered = new List<Produkter>();
            //loop through items and add to items ordered
            foreach (var item in items)
            {
                //check if item is already in list
                var itemInList = itemsOrdered.FirstOrDefault(x => x.id == item.id);
                if (itemInList != null)
                {
                    //if item is in list, increase quantity
                    itemInList.quantity += item.quantity;
                }
                else
                {
                    //if item is not in list, add to list
                    itemsOrdered.Add(item);
                }
                
            }
            return JsonSerializer.Serialize<List<Produkter>>(itemsOrdered);

        }
    }
}
