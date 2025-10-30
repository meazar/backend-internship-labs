using System;
using Microsoft.Data.SqlClient;

namespace InventoryManagementSystem;

class Program
{
    static void Main(string[] args)
    {
        ProductRepository repo = new ProductRepository();

        while (true)
        {
            Console.WriteLine("\n---------Product Inventory System------------");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. View All Products");
            Console.WriteLine("3. Update All Products");
            Console.WriteLine("4. Delete the Product");
            Console.WriteLine("5. Search Product By Category");
            Console.WriteLine("6. Exit");
            Console.WriteLine("Choose an option");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Products p = new Products();
                    Console.WriteLine("Enter the product name");
                    p.ProductName = Console.ReadLine();
                    Console.WriteLine("Enter Category");
                    p.Category = Console.ReadLine();
                    Console.WriteLine("Enter Quantity");
                    p.Quantity = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter Price");
                    p.Price = int.Parse(Console.ReadLine());

                    repo.AddProduct(p);
                    Console.WriteLine("Product added Successfully");
                    break;

                case 2:
                    List<Products> all = repo.GetAllProducts();
                    Console.WriteLine("ProductId \t ProductName \t Category \t Quantity \t Price");
                    foreach (var item in all)
                        Console.WriteLine($"{item.ProductID,-16} {item.ProductName,-15} {item.Category,-15} {item.Quantity,-15} Rs.{item.Price}");
                    break;
                case 3:
                    Console.WriteLine("Enter id of the product you want to update: ");
                    int id = int.Parse(Console.ReadLine());
                    Products upd = new Products { ProductID = id };
                    Console.WriteLine("New Name: ");
                    upd.ProductName = Console.ReadLine();
                    Console.WriteLine("New Category: ");
                    upd.Category = Console.ReadLine();
                    Console.WriteLine("New Quantity");
                    upd.Quantity = int.Parse(Console.ReadLine());
                    Console.WriteLine("New Price: ");
                    upd.Price = decimal.Parse(Console.ReadLine());
                    repo.UpdateProduct(upd);
                    Console.WriteLine("Product updated successfully");
                    break;
                case 4:
                    Console.WriteLine("Enter Product ID to Delete: ");
                    int delId = int.Parse(Console.ReadLine());
                    repo.DeleteProduct(delId);
                    break;
                case 5:
                    Console.WriteLine("Enter the category to search");
                    string cat = Console.ReadLine();
                    List<Products> search = repo.SearchByCategory(cat);
                    Console.WriteLine("ProductID \t ProductName \t Price");
                    foreach (var item in search)
                        Console.WriteLine($"{item.ProductID,-16}{item.ProductName,-15}  Rs.{item.Price}");
                    break;
                case 6:
                    return;
                default:
                    Console.WriteLine("Invalid choice");
                    break;

            
            }
        }
    }

}