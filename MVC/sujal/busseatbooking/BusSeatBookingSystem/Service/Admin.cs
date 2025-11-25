using BusSeatBookingSystem.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusSeatBookingSystem.Service
{
    public class Admin
    {
        private readonly DatabaseServer _dbServer;

        public Admin(DatabaseServer dbServer)
        {
            _dbServer = dbServer;
        }


        public List<Bus> GetAllBuses()
        {
            List<Bus> Buses = new List<Bus>();
            var connection = _dbServer.GetConnection();
            connection.Open();
            var query = "SELECT BusId, BusNumber, BusType, TotalSeats FROM Buses ORDER BY BusNumber";
            var command = new SqlCommand(query, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Buses.Add(new Bus
                {
                    BusId = (int)reader["BusId"],
                    BusNumber = reader["BusNumber"].ToString(),
                    BusType = reader["BusType"].ToString(),
                    TotalSeats = (int)reader["TotalSeats"]
                });
            }
            return Buses;


        }

        public List<Bus> GetAvailableBuses()
        {
            return GetAllBuses();
        }

       

        public List<Routes> GetAllRoutes()
        {
            var routes = new List<Routes>();
            using var connection = _dbServer.GetConnection();
            connection.Open();
            var query = "SELECT RouteId, FromLocation, ToLocation, DistanceKm, DurationMinutes, TicketPrice FROM Routes ORDER BY FromLocation";
            var command = new SqlCommand(query, connection);
            var reader = command.ExecuteReader();
            while(reader.Read())
            {
                routes.Add(new Routes
                {
                    FromLocation = reader["FromLocation"].ToString(),
                    ToLocation = reader["ToLocation"].ToString(),
                    RouteId = (int)reader["RouteId"],
                    DistanceKm = reader["DistanceKm"].ToString(),
                    DurationMinutes = (int)reader["DurationMinutes"],
                    TicketPrice = (decimal)reader["TicketPrice"]

                });

            }
            return routes;
        }

        public List<Routes> GetAvailableRoutes()
        {
            return GetAllRoutes();
        }


        public List<Schedule> GetAllSchedules()
        {
            var schedules = new List<Schedule>();
            using var connection = _dbServer.GetConnection();
            connection.Open();

            var query = @"SELECT s.ScheduleId, s.BusId, s.RouteId, b.BusNumber,s.DepartureTime,s.ArrivalTime, s.AvailableSeats,b.BusType,r.FromLocation,r.ToLocation,r.TicketPrice FROM Schedule ORDER BY DepartureTime";
            var command = new SqlCommand(query, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                schedules.Add(new Schedule
                {
                    ScheduleId = (int)reader["ScheduleId"],
                    BusId = (int)reader["BusId"],
                    RouteId = (int)reader["RouteId"],
                    BusNumber = reader["BusNumber"].ToString(),
                    DepartureTime = (DateTime)reader["DepartureTime"],
                    ArrivalTime = (DateTime)reader["ArrivalTime"],
                    AvailableSeats = (int)reader["AvailableSeats"],
                    FromLocation = reader["FromLocation"].ToString(),
                    ToLocation = reader["ToLocation"].ToString(),
                    TicketPrice = (decimal)reader["TicketPrice"]


                     
                     
                });
                

            }
            return schedules;
        }
        public bool AddBus(string busNumber, string busType,int totalSeats)
        {
            try
            {
                using var connection = _dbServer.GetConnection();
                connection.Open();

                var checkQuery = "SELECT COUNT(*) FROM Buses WHERE BusNumber = @BusNumber";
                using var checkcommand = new SqlCommand(checkQuery, connection);
                checkcommand.Parameters.AddWithValue("@BusNumber", busNumber);
                var existingCount = (int)checkcommand.ExecuteScalar();

                if (existingCount > 0)
                {
                    Console.WriteLine($"!!!!!!!!!!!This BusNumber '{busNumber}'is Already Exists!!!!!!!! ");
                    return false;

                }


                var query = "INSERT INTO Buses (BusNumber,busType, TotalSeats) VALUES (@BusNumber,@busType, @TotalSeats)";
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BusNumber", busNumber);
                command.Parameters.AddWithValue("@busType", busType);
                command.Parameters.AddWithValue("@TotalSeats", totalSeats);
                command.ExecuteNonQuery();

                CreateSeatsForBus(connection, busNumber);

                return true;

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to add bus:{ ex.Message}");

                return false;
            }

            

        }

        public void CreateSeatsForBus(SqlConnection connection, string busNumber)
        {
            // Retrieve the BusId of the newly added bus
            var getBusIdQuery = "SELECT BusId FROM Buses WHERE BusNumber = @BusNumber";
            using var getBusIdCommand = new SqlCommand(getBusIdQuery, connection);
            getBusIdCommand.Parameters.AddWithValue("@BusNumber", busNumber);
            var busId = (int)getBusIdCommand.ExecuteScalar();

            for(int row=1; row<=10; row++)
            {
                for(char col='A'; col<='D'; col++)
                {
                    var seatQuery = "INSERT INTO Seats (SeatNumber,busId) VALUES (@SeatNumber, @BusId)";

                    using var seatCommand = new SqlCommand(seatQuery, connection);
                    seatCommand.Parameters.AddWithValue("@SeatNumber", $"{row}{col}");
                    seatCommand.Parameters.AddWithValue("BusId", busId);
         
                    seatCommand.ExecuteNonQuery();


                }
            }
        }

        public void AddRoute(string fromLocation, string toLocation,decimal distance, int durationMinutes ,decimal ticketPrice)
        {
            using var connection = _dbServer.GetConnection();
            connection.Open();
            var query = @"INSERT INTO Routes (FromLocation, ToLocation, DistanceKm,DurationMinutes, TicketPrice) VALUES (@FromLocation, @ToLocation,@DistanceKm,@DurationMinutes,@TicketPrice)";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@FromLocation", fromLocation);
            command.Parameters.AddWithValue("@ToLocation", toLocation);
            command.Parameters.AddWithValue("@DistanceKm", distance);
            command.Parameters.AddWithValue("@DurationMinutes", durationMinutes);
            command.Parameters.AddWithValue("@TicketPrice", ticketPrice);

            command.ExecuteNonQuery();


        }


        public void AddSchedule(int busId, int routeId,DateTime departureTime,DateTime arrivalTime)
        {
            using var connection = _dbServer.GetConnection();

            connection.Open();

            var getSeatsQuery = "SELECT TotalSeats FROM Buses WHERE BusId = @BusId";
            using var getCommand = new SqlCommand(getSeatsQuery, connection);
            getCommand.Parameters.AddWithValue("@BusId", busId);
            var totalSeats = (int)getCommand.ExecuteScalar();


            var query = @"INSERT INTO Schedule (BusId, RouteId, DepartureTime, ArrivalTime, AvailableSeats) VALUES (@BusId, @RouteId, @DepartureTime, @ArrivalTime, @AvailableSeats)";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@BusId", busId);
            command.Parameters.AddWithValue("@RouteId", routeId);
            command.Parameters.AddWithValue("@DepartureTime", departureTime);
            command.Parameters.AddWithValue("@ArrivalTime", arrivalTime);
            command.Parameters.AddWithValue("@AvailableSeats", totalSeats);

            command.ExecuteNonQuery();


        }

        public List<Booking> GetAllBooking()
        {
            var booking = new List<Booking>();

            using var connection = _dbServer.GetConnection();

            connection.Open();

            var query = @"SELECT b.BookingId,b.UserId,b.ScheduleId,b.SeatId,b.BookingTime,b.Status,u.Name as UserName,r.FromLocation,r.ToLocation,sc.DepartureTime,st.SeatNumber
                        FROM Bookings b 
                        INNER JOIN Users u ON b.UserId = u.UserId
                        INNER JOIN Schedule sc ON b.ScheduleId = sc.ScheduleId
                        INNER JOIN Routes r ON sc.RouteId=r.RouteId
                        INNER JOIN Seats st ON b.SeatId = st.SeatId
                        ORDER BY b.BookingTime DESC";

            using var command = new SqlCommand(query, connection);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                booking.Add(new Booking
                {
                    BookingId = (int)reader["BookingId"],
                    UserId = (int)reader["UserId"],
                    ScheduleId = (int)reader["ScheduleId"],
                    SeatId = (int)reader["SeatId"],
                    BookingTime = (DateTime)reader["BookingTime"],
                    FromLocation = reader["FromLocation"].ToString(),
                    ToLocation = reader["ToLocation"].ToString(),
                    Status = reader["Status"].ToString(),
                    UserName = reader["UserName"].ToString(),
                    DepartureTime = (DateTime)reader["DepartureTime"],
                    SeatNumber = reader["SeatNumber"].ToString()
                });
            }
            return booking;
        }


    }
}
