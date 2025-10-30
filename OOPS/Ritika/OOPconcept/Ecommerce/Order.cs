using System;
using System.Collections.Generic;

namespace Ecommerce
{
    public class Order
    {
        public Customer Customer { get; set; }
        private List<Product> products = new List<Product>();
        private List<int> quantities = new List<int>();
        public Order(Customer customer)
        {
            Customer = customer;
        }
        public void AddProduct(Product product, int quantity)
        {
            if (product.Purchase(quantity))
            {
                products.Add(product);
                quantities.Add(quantity);
            }
        }
        public void Checkout()
        {
            double total = 0;
            for (int i = 0; i < products.Count; i++)
            {
                total += products[i].Price * quantities[i];
            }
            double discount = Customer.GetDiscount(total);
            double finalAmount = total - discount;

            Console.WriteLine($"\nCustomer: {Customer.Name}");
            Console.WriteLine($"Total: {total:C}");
            Console.WriteLine($"Discount: {discount:C}");
            Console.WriteLine($"Amount to Pay: {finalAmount:C}");
        }
    }
}
