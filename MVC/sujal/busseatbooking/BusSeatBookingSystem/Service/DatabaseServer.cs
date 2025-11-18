using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusSeatBookingSystem.Service
{
    public class DatabaseServer
    {
        private readonly string _connectionString;

        public DatabaseServer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection (_connectionString);
        }   
    }
}
