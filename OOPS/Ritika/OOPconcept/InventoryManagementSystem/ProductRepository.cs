using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace InventoryManagementSystem
{
    public class ProductRepository
    {
        private string connectionString = "Server=Ritika\\MSSQLServer01;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True";

        public void AddProduct(Products p)
        {
            using (SqlConnection conn = new SqlConnection(connectionString)) 
            {
                conn.Open();
                string query = "INSERT INTO Products(ProductName,Category,Quantity,Price) values (@Name, @Category, @Quantity, @Price)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", p.ProductName);
                cmd.Parameters.AddWithValue("@Category", p.Category);
                cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                cmd.Parameters.AddWithValue("@Price", p.Price);
                cmd.ExecuteNonQuery();

            }
        }

        public List<Products> GetAllProducts() 
        {
            List<Products> list = new List<Products>();
            using (SqlConnection conn = new SqlConnection(connectionString)) 
            {
                conn.Open();
                string query = "SELECT * FROM Products";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) 
                {
                    list.Add(new Products
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = reader["ProductName"].ToString(),
                        Category = reader["Category"].ToString(),
                        Quantity = (int)reader["Quantity"],
                        Price= (decimal)reader["Price"],
                        AddedDate = (DateTime)reader["AddedDate"]
                    });

                }
            }
            return list;
        }

        public void UpdateProduct(Products p)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Products SET ProductName=@Name, Category=@Category, Quantity=@Quantity, Price=@Price WHERE ProductID=@ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", p.ProductName);
                cmd.Parameters.AddWithValue("@Category", p.Category);
                cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                cmd.Parameters.AddWithValue("@Price", p.Price);
                cmd.Parameters.AddWithValue("@ID", p.ProductID);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Products WHERE ProductID= @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Products> SearchByCategory(string category) 
        {
            List<Products> list = new List<Products>();
            using (SqlConnection conn = new SqlConnection(connectionString)) 
            {
                conn.Open();
                string query = "SELECT * FROM Products WHERE Category=@Category";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Category", category);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) 
                {
                    list.Add(new Products
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = (string)reader["ProductName"],
                        Category = (string)reader["Category"],
                        Quantity = (int)reader["Quantity"],
                        Price = (decimal)reader["Price"]
                    });


                }
            }
            return list;
        }
    }
}
