using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using PetAdoptionManagementSystem.Data;
using PetAdoptionManagementSystem.Models;

namespace PetAdoptionManagementSystem.Services
{
    public class PetService
    {
        private readonly DatabaseHelper _db;

        public PetService(DatabaseHelper db)
        {
            _db = db;
        }

        public void AddPet(Pet pet)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                string query = @"Insert Into Pets (Name, Species, Breed, Age,Gender, IsAdopted)
                                 Values (@Name, @Species, @Breed, @Age, @Gender, @IsAdopted)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", pet.Name);
                cmd.Parameters.AddWithValue("@Species", pet.Species);
                cmd.Parameters.AddWithValue("@Breed", pet.Breed);
                cmd.Parameters.AddWithValue("@Age", pet.Age);
                cmd.Parameters.AddWithValue("@Gender", pet.Gender);
                cmd.Parameters.AddWithValue("@IsAdopted", pet.IsAdopted);

                int row = cmd.ExecuteNonQuery();
                Console.WriteLine(row> 0? "Pet Added Successfully.": "Failed to add pet");
            }
        }

        public List<Pet> GetAvailablePets()
        {
            var pets = new List<Pet>();
            using(SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                string query = "Select * from Pets Where IsAdopted = 0";
                SqlCommand cmd = new SqlCommand(query,conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    pets.Add(new Pet
                    {
                        PetId = Convert.ToInt32(reader["PetId"]),
                        Name = reader["Name"].ToString(),
                        Species = reader["Species"].ToString(),
                        Breed = reader["Breed"].ToString(),
                        Age = Convert.ToInt32(reader["Age"]),
                        Gender = reader["Gender"].ToString(),
                        IsAdopted = Convert.ToBoolean(reader["IsAdopted"]),
                        DateAdded = Convert.ToDateTime(reader["DateAdded"])
                    });
                }
            }
            return pets;
        }

        public void MarkAsAdopted(int id)
        {
            using(SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                string query = "Update Pets set IsAdopted= 1 where PetId= @PetId";
                SqlCommand cmd = new SqlCommand(query,conn);
                cmd.Parameters.AddWithValue("@PetId", id);
                cmd.ExecuteNonQuery();
            }
        }

    }

}
