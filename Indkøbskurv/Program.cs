using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Indkøbskurv
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
           
        }
        static Kunder CreateCustomer()
        {
            //new customer
            Kunder kunde = new Kunder();
            //new shoping-bag
            Indkøbskurv indkøbskurv = new Indkøbskurv();
            //assign shopping bag to customer
            kunde.indkøbskurv = indkøbskurv;
            return kunde;
        }
        static Produkter GetProducts()
        {
            //get json from file
            string filename = "Products.json";
            string path = Path.Combine(Environment.CurrentDirectory, filename);
            using (StreamReader sr = new StreamReader(path))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive=true;
                }
            }
        Produkter p = new Produkter();
        return
        }
    }
}
