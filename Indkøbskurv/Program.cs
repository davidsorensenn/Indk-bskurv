using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Dynamic;
using System.Net.Sockets;

namespace Indkøbskurv
{
    internal class Program
    {
        public static List<Produkt> items = GetProductsFromFilename("Products.json");
        static void Main(string[] args)
        {
            //create customer
            Kunde kunde = CreateCustomer();
            while (true)
            {
                bool valid = false;
                string mode = "";
                while (!valid)
                {
                    Console.Clear();
                    Console.WriteLine("Select mode [Catalog: a | Shopping bag: b]");
                    mode = Console.ReadLine();
                    if (new[] { "a", "b", }.Contains(mode.ToLower()))
                    {
                        valid = true;
                    }
                }

                switch (mode)
                {
                    case "a":
                        //Add to cart
                        foreach (Produkt item in items)
                        {
                            Console.WriteLine(item.Title + " | " + item.Price + "kr. | "+item.Quantity);
                        }
                        Console.WriteLine(Environment.NewLine + "Do you want to buy something [yes | no]");
                        string choice = Console.ReadLine();
                        if(choice =="yes"||choice == "y")
                        {
                            AddToCart(kunde);
                            Console.ReadKey();
                        }
                       
                        break;
                    case "b":
                        List<Produkt> Products = kunde.indkøbskurv.GetShoppinglist();
                        foreach (Produkt item in Products)
                        {
                            Console.WriteLine(item.Title + " | " + item.Price + "kr. | " + item.Quantity);
                        }
                        RemoveFromCart(kunde);
                        break;
                }

            }
        }

        static Kunde CreateCustomer()
        {
            //new customer
            Kunde kunde = new Kunde();
            //new shoping-bag
            Indkøbskurv indkøbskurv = new Indkøbskurv();
            //assign shopping bag to customer
            kunde.indkøbskurv = indkøbskurv;
            return kunde;
        }
        static List<Produkt> GetProductsFromFilename(string filename)
        {
            //get json from file
            string path = Path.Combine(Environment.CurrentDirectory, filename);
            using (StreamReader sr = new StreamReader(path))
            {
                string productsJson = sr.ReadToEnd();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                Produkt p = new Produkt();
                return JsonSerializer.Deserialize<List<Produkt>>(productsJson, options);
            }
        }
        static void AddToCart(Kunde kunde)
        {
            //choose Produkt to Indkøbskurv
            bool valid = false;
            while (!valid)
            {
                Console.WriteLine("What do you want to buy?");
                string input = Console.ReadLine();
                int index = 0;
                int.TryParse(input, out index);
                if (index != 0 && index <= items.Count)
                {
                    index--;
                    kunde.indkøbskurv.AddtoShoppinglist(items[index]);
                    valid = true;
                    Console.Clear();
                    Console.WriteLine("{0} was added to cart.", items[index].Title);
                    if (items[index].Quantity > 1)
                    {
                        items[index].Quantity--;
                    }
                    else if (items[index].Quantity == 1)
                    {
                        items.RemoveAt(index);
                    }
                }
            }
            List<Produkt> produkter = kunde.indkøbskurv.GetShoppinglist();
            
            
        }
        static void RemoveFromCart(Kunde kunde)
        {
            //choose Produkt
            bool valid = false;
            while (!valid)
            {
                Console.WriteLine("What do you want to remove?");
                string input = Console.ReadLine();
                int index = 0;
                List<Produkt> listOfProducts = kunde.indkøbskurv.GetShoppinglist();

                int.TryParse(input, out index);
                if (index != 0 && index <= listOfProducts.Count)
                {
                    index--;
                    Produkt removedItem = listOfProducts[index];
                    kunde.indkøbskurv.RemoveFromShoppinglist(index);
                    valid = true;
                    Console.Clear();
                    Console.WriteLine("{0} was removed from cart.",removedItem.Title);
                    bool exists = false;
                   
                    foreach(Produkt item in items)
                    {
                        if (item.Id == removedItem.Id)
                        {
                            exists = true;
                            item.Quantity++;
                        }
                    }
                    if (exists == false)
                    {
                        items.Add(removedItem);
                    }
                }
                else { break;}
            }
            List<Produkt> produkter = kunde.indkøbskurv.GetShoppinglist();


        }
    }
}
