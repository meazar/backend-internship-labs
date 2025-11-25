using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace PetAdoptionManagementSystem.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;
        static DatabaseHelper()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        }

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");


            if(string.IsNullOrEmpty(_connectionString) )
            {
                throw new InvalidOperationException("Connection string is missing or empty");
            }
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
