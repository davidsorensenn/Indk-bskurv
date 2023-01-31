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
        //states
        public static List<Produkt> Products = GetProductsFromFilename("Products.json");
        
        public static List<Produkt> shoppingBag = new List<Produkt>();
        public static Kunde kunde = CreateCustomer();

        static void Main(string[] args)
        {
            //create customer
            
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
                        DisplayItemsInProducts();
                        
                        Console.WriteLine(Environment.NewLine + "Do you want to buy something [yes | no]");
                        string choice = Console.ReadLine();
                        if(choice =="yes"||choice == "y")
                        {
                            Console.Clear();

                            int id = GetIdFromProductsIndex();
                            int amount = GetAmount();
                            AddToCart(id, amount);
                            foreach(Produkt p in shoppingBag)
                            {
                                Console.WriteLine(p.Title+" | " + p.Quantity);

                            }
                            Console.ReadKey();
                        }
                       
                        break;
                    case "b":
                        
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
        static void DisplayItemsInProducts(){
            foreach (Produkt item in Products)
            {
                Console.WriteLine(item.Title + " | " + item.Price + "kr. | " + item.Quantity);
            }
        }
        static void AddToCart(int id, int amount)
        {
            //loop through products
            foreach (Produkt item in Products)
            {
                //check if id matches
                if (item.Id == id)
                {
                    //check if item is in stock
                    if (item.Quantity >= amount)
                    {
                        //if item is in ShoppingBag increese quantity by amount
                        var itemInList = shoppingBag.FirstOrDefault(x => x.Id == item.Id);
                        //store the item in variable and manage quantity
                        if (itemInList != null)
                        {
                            //if item is in ShoppingBag increese quantity by amount
                            itemInList.Quantity+=amount;
                        }
                        else
                        {
                            //if item is not in ShoppingBag set quantity and add item
                            Produkt transaktion = new Produkt();
                            transaktion.Title = item.Title;
                            transaktion.Price = item.Price;
                            transaktion.Id = item.Id;
                            transaktion.Quantity = amount;
                            shoppingBag.Add(transaktion);
                        }
                        //Decrees quantity of item in stock by amount
                        item.Quantity -= amount;
                        Console.WriteLine("{0} was added to cart.", item.Title);
                    }
                    else
                    {
                        Console.WriteLine("Out of stock");
                    }
                    break;
                }
                
            }
        }
        static int GetIdFromProductsIndex()
        {
            int output = -1;
            bool valid = false;
            while (!valid)
            {
               
                DisplayItemsInProducts();
                Console.WriteLine(Environment.NewLine + "Select item by index or full product name");
                string input = Console.ReadLine();
                int index;
                bool isInt = int.TryParse(input, out index);
                if(isInt == true && index < Products.Count)
                {
                    output = Products[index].Id;
                    valid = true;


                }
                else 
                {
                    
                    var itemInList = Products.FirstOrDefault(x => x.Title.ToLower() == input.ToLower().Trim());
                    if (itemInList != null)
                    {
                        output = itemInList.Id;
                        valid = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("We did not find a mach to \"{0}\"" , input);
                        Console.WriteLine("");
                    }
                }
                

            }
            return output;
        }

        static int GetAmount()
        {
            int output = 0;
            bool valid = false;
            while (!valid)
            {
                Console.WriteLine("How many do you want?");
                string input = Console.ReadLine();
                int amount;
                bool isInt = int.TryParse(input, out amount);
                if (isInt == true)
                {
                    output = amount;
                    valid = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Please type a real number");
                }


            }
            return output;
        }
    }
}
