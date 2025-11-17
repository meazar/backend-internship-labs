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
    public class AdoptionService
    {
        private readonly DatabaseHelper _db;
        private readonly PetService _petService;
           
        public AdoptionService(DatabaseHelper db, PetService petService)
        {
            _db = db;
            _petService = petService;
        }

        public void AdoptPet(int petId, int userId)
        {
            using(SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                string query = @"Insert into Adoptions (PetId, UserId)
                                    values (@PetId, @UserId)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PetId", petId);
                cmd.Parameters.AddWithValue("@userId", userId);

                int rows = cmd.ExecuteNonQuery();
                if(rows> 0)
                {
                    _petService.MarkAsAdopted(petId);
                    Console.WriteLine("Adoption successful");
                }
                else
                {
                    Console.WriteLine("Failed to Adopt pet");
                }
            }
        }

        public List<Adoption> GetAdoptionsByUser(int userId)
        {
            var adoptions = new List<Adoption>();
            using(SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                string query = "Select * from Adoptions where UserId = @UserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    adoptions.Add(new Adoption
                    {
                        AdoptionId = Convert.ToInt32(reader["AdoptionId"]),
                        PetId = Convert.ToInt32(reader["PetId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        AdoptionDate = Convert.ToDateTime(reader["AdoptionDate"])
                    });
                }
            }
            return adoptions;
        }

        public List<AdoptionDetails> GetAllAdoptions()
        {
            var adoptions = new List<AdoptionDetails>();
            using(SqlConnection conn = _db.GetConnection())
            {
                conn.Open();    
                string query = @"Select a.AdoptionId, p.Name as PetName, u.FullName as AdopterName, a.AdoptionDate
                                From Adoptions a
                                Inner Join Pets p on a.PetId = p.PetId
                                Inner Join Users u on a.UserId = u.UserId
                                Order by a.AdoptionDate Desc";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    adoptions.Add(new AdoptionDetails
                    {
                        AdoptionId = Convert.ToInt32(reader["AdoptionId"]),
                        PetName = reader["PetName"].ToString(),
                        AdopterName = reader["AdopterName"].ToString(),
                        AdoptionDate = Convert.ToDateTime(reader["AdoptionDate"])
                    });
                }
            }
            return adoptions;
        }

    }
}
