using System;

namespace Ecommerce;

class Program
{
    static void Main(string[] args)
    {
     
        Product laptop = new Product("Laptop", 1000, 5);
        Product phone = new Product("Phone", 500, 10);

     
        Customer regular = new Customer("Mastermind");
        PremiumCustomer premium = new PremiumCustomer("Mysterious");
        VIPCustomer vip = new VIPCustomer("Strange");

        Order order1 = new Order(regular);
        order1.AddProduct(laptop, 6);
        order1.AddProduct(phone, 2);
        order1.Checkout();

        Order order2 = new Order(vip);
        order2.AddProduct(laptop, 2);
        order2.Checkout();
    }
}
