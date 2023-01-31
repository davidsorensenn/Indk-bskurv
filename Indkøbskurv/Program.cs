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
                    
                    Console.WriteLine("Select mode [Catalog: a | Shopping bag: b]");
                    mode = Console.ReadLine();
                    if (new[] { "a", "b", }.Contains(mode.ToLower()))
                    {
                        valid = true;
                    }
                    Console.Clear();
                }

                switch (mode)
                {
                    case "a":
                        //Add to cart
                        DisplayItemsInProducts();

                        Console.WriteLine(Environment.NewLine + "Do you want to buy something [yes | no]");
                        string choice = Console.ReadLine();
                        if (choice == "yes" || choice == "y")
                        {
                            Console.Clear();

                            int id = GetIdFromProductsIndex();
                            int amount = GetAmount();
                            AddToCart(id, amount);
                            DisplayItemsInShoppingBag();

                            Console.ReadKey();
                        }

                        break;
                    case "b":
                        if (shoppingBag.Count == 0)
                        {
                            Console.WriteLine("Your shopping bag is empty");
                        }
                        else
                        {
                            DisplayItemsInShoppingBag();
                            Console.WriteLine(Environment.NewLine + "Do you want to remove something [yes | no]");
                            string choice1 = Console.ReadLine();
                            if (choice1 == "yes" || choice1 == "y")
                            {
                                

                                int id = GetIdFromShoppingBagIndex();
                                int amount = GetAmount();
                                RemoveFromCart(id, amount);
                                DisplayItemsInProducts();
                                Console.ReadKey();

                            }
                        }

                        break;
                }

            }
        }

        static Kunde CreateCustomer()
        {
            //new customer
            Kunde kunde = new Kunde();
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
        static void DisplayItemsInProducts()
        {
            foreach (Produkt item in Products)
            {
                Console.WriteLine(item.Title + " | " + item.Price + "kr. | " + item.Quantity);
            }
        }
        static void DisplayItemsInShoppingBag()
        {
            foreach (Produkt item in shoppingBag)
            {
                Console.WriteLine(item.Title + " | " + item.Price + "kr. | " + item.Quantity);
            }
        }
        static void RemoveFromCart(int id, int amount)
        {
            foreach (Produkt item in shoppingBag)
            {
                //if id matches id in ShoppingBag
                if (item.Id == id)
                {
                    //get relative item from Products
                    var itemInList = Products.FirstOrDefault(x => x.Id == item.Id);
                    if (item.Quantity > amount)
                    {
                        item.Quantity -= amount;
                        Console.WriteLine("{0} was removed from cart.", item.Title);
                        itemInList.Quantity += amount;

                    }
                    else if (item.Quantity == amount)
                    {
                        shoppingBag.Remove(item);
                        itemInList.Quantity += amount;
                    }
                    else
                    {
                        Console.WriteLine("You can't remove more than you have in cart.");
                    }
                    break;
                }
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
                            itemInList.Quantity += amount;
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
                //if input is a number isInt will be true and index will be the number
                bool isInt = int.TryParse(input, out index);
                //if input is a number and index is in range of products
                if (isInt == true && index < Products.Count)
                {
                    //get id from products index and set output to id
                    output = Products[index].Id;
                    //break loop
                    valid = true;
                }
                //if input is not a number. try to find a match in products
                else
                {
                    //check if input matches a product
                    var itemInList = Products.FirstOrDefault(x => x.Title.ToLower() == input.ToLower().Trim());
                    //if match is found set output to id
                    if (itemInList != null)
                    {
                        output = itemInList.Id;
                        //break loop
                        valid = true;
                    }
                    //display error message and try again
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("We did not find a mach to \"{0}\"", input);
                        Console.WriteLine("");
                    }
                }
            }
            return output;
        }
        static int GetIdFromShoppingBagIndex()
        {
            int output = -1;
            bool valid = false;
            while (!valid)
            {

                DisplayItemsInShoppingBag();
                Console.WriteLine(Environment.NewLine + "Select item by index or full product name");
                string input = Console.ReadLine();
                int index;
                //if input is a number isInt will be true and index will be the number
                bool isInt = int.TryParse(input, out index);
                //if input is a number and index is in range of products
                if (isInt == true && index < shoppingBag.Count)
                {
                    //get id from products index and set output to id
                    output = shoppingBag[index].Id;
                    //break loop
                    valid = true;
                }
                //if input is not a number. try to find a match in products
                else
                {
                    //check if input matches a product
                    var itemInList = shoppingBag.FirstOrDefault(x => x.Title.ToLower() == input.ToLower().Trim());
                    //if match is found set output to id
                    if (itemInList != null)
                    {
                        output = itemInList.Id;
                        //break loop
                        valid = true;
                    }
                    //display error message and try again
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("We did not find a mach to \"{0}\"", input);
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
                //if input is a number isInt will be true and amount will be the return number 
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
