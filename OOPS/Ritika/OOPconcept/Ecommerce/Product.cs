using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    public class Product
    {
        private int stock;
        public string Name { get; set; }
        public int Price { get; set; }

        public Product(string name, int price, int stock)
        {
            Name = name;
            Price = price;
            this.stock = stock;
        }
        public bool Purchase(int quality)
        {
            if (quality <= 0)
            {
                Console.WriteLine("Quality should be in positive");
                return false;
            }
            if (quality > stock) 
            {
                Console.WriteLine("Out of Stock!");
                return false;
            }
            stock -= quality;
            Console.WriteLine($"{quality} {Name}(s) purchased. Remaining Products: {stock}");

            return true;
        }
        public int GetStock() 
        {
            return stock;
        }
    }
}
