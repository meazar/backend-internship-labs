using System;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;

namespace StudentAttendanceTracker;
class Program
{
    private static string connectionString = "Server=Ritika\\MSSQLSERVER01;Database=AttendanceDB;Trusted_Connection=True;TrustServerCertificate=True;";

     static void Main(string[] args)
     {
        while (true)
        {
            Console.WriteLine("\n---------Student Attendance Tracker--------");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. Update Student");
            Console.WriteLine("3. Delete Student");
            Console.WriteLine("4. View Student");
            Console.WriteLine("5. Mark Attendance");
            Console.WriteLine("6. View Attendance");
            Console.WriteLine("7. Exit");
            Console.WriteLine("Choose an option");

            var choice=Console.ReadLine();

            switch (choice) 
            {
                case "1":
                    AddStudent();
                    break;
                case "2":
                    UpdateStudent();
                    break;
                case "3":
                    DeleteStudent(); 
                    break;
                case "4":
                    ViewStudents();
                    break;
                case "5":
                    MarkAttendance(); 
                    break;
                case "6":
                    ViewAttendance();
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }
    static void AddStudent()
    {
        Console.WriteLine("Enter student name:");
        string name = Console.ReadLine();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "INSERT INTO Students(name) VALUES (@name)";
            SqlCommand cmd = new SqlCommand(query,conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Student added successfully");
        }
        
    }

    static void UpdateStudent()
    {
        Console.WriteLine("Enter student id: ");
        int id = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter new name: ");
        string newname = Console.ReadLine();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "UPDATE Students SET name= @name where StudentID = @id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", newname);
            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Student updated successfully." : "Student not found");
        }
    }

    static void DeleteStudent()
    {
        Console.WriteLine("Enter the id you want to delete: ");
        int id = int.Parse(Console.ReadLine());

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string attendquery = "DELETE FROM Attendance WHERE StudentID= @id";
            SqlCommand command = new SqlCommand(attendquery, conn);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            string query = "DELETE FROM Students WHERE StudentID = @id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Student Deleted" : "Student not found");
        }
    }

    static void ViewStudents()
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT * FROM Students";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine("-----Students---------");
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["StudentID"]}, Name: {reader["Name"]}");
            }
        }
    }

    static void MarkAttendance()
    {
        Console.WriteLine("Enter student ID:");
        int id = int.Parse(Console.ReadLine());
        Console.WriteLine("ENter status (Present/Absent):");
        string status = Console.ReadLine();

        using (SqlConnection conn = new SqlConnection(connectionString)) 
        {
            conn.Open();
            string query = "INSERT INTO Attendance (StudentID,Status) VALUES (@id,@status)";
            SqlCommand cmd = new SqlCommand(query,conn);
            cmd.Parameters.AddWithValue("@id",id);
            cmd.Parameters.AddWithValue ("@status", status);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Attendance Marked Successfully");

        }
        
    }

    static void ViewAttendance()
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = @"
                SELECT s.Name, a.Date, a.Status
                FROM Students s
                JOIN Attendance a ON s.StudentID = a.StudentID
                ORDER BY a.Date DESC
                ";
            SqlCommand cmd = new SqlCommand(@query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine("\n Attendance Records: ");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]} - {reader["Date"]} - {reader["Status"]}");
            }
        }
    }

}
