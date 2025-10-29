using System;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace LibraryManagementSystem
{
    public static class Database
    {
        private static string libraryDbConnection = "Server=Ritika\\MSSQLSERVER01;Database=LibraryDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(libraryDbConnection);
        }
    }
}
