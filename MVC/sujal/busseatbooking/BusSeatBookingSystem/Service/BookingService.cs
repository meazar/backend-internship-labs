using BusSeatBookingSystem.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusSeatBookingSystem.Service
{
    public class BookingService
    {
        private readonly DatabaseServer _dbServer;

        public BookingService(DatabaseServer dbServer)
        {
            _dbServer = dbServer;
        }

        public List<Schedule> GetAvailableSchedules(string fromLocation = null, string toLocation = null)
        {
            var schedules = new List<Schedule>();

            using var connection = _dbServer.GetConnection();
            connection.Open();

            var query = @"
                SELECT s.ScheduleId, s.BusId, s.RouteId, s.DepartureTime, s.ArrivalTime, s.AvailableSeats,
                       b.BusNumber, r.FromLocation, r.ToLocation, r.TicketPrice
                FROM Schedule s 
                INNER JOIN Buses b ON s.BusId = b.BusId
                INNER JOIN Routes r ON s.RouteId = r.RouteId
                WHERE s.AvailableSeats > 0
                AND (@FromLocation IS null OR r.FromLocation LIKE '%' + @FromLocation +'%')
                AND (@ToLocation IS null OR r.ToLocation LIKE '%' + @ToLocation +'%')
               ORDER BY s.DepartureTime";



            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FromLocation", fromLocation ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ToLocation", toLocation ?? (object)DBNull.Value);

            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {

                schedules.Add(new Schedule
                {
                    ScheduleId = Convert.ToInt32(reader["ScheduleId"]),
                    BusId = Convert.ToInt32(reader["BusId"]),
                    RouteId = Convert.ToInt32(reader["RouteId"]),
                    DepartureTime = Convert.ToDateTime(reader["DepartureTime"]),
                    ArrivalTime = Convert.ToDateTime(reader["ArrivalTime"]),
                    AvailableSeats = Convert.ToInt32(reader["AvailableSeats"]),
                    BusNumber = reader["BusNumber"].ToString(),
                    FromLocation = reader["FromLocation"].ToString(),
                    ToLocation = reader["ToLocation"].ToString(),
                    TicketPrice = Convert.ToDecimal(reader["TicketPrice"])
                });
                
            }
            return schedules;
        }

        public List<Seat> GetAvailableSeats(int scheduleId)
        {
            var seats = new List<Seat>();

            using var connection = _dbServer.GetConnection();
            connection.Open();

            var query = @"
                SELECT st.SeatId, st.BusId, st.SeatNumber, st.IsBooked
                FROM Seats st
                INNER JOIN Schedule s ON st.BusId = s.BusId
                WHERE s.scheduleId = @ScheduleId AND IsBooked = 0";
                
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ScheduleId", scheduleId);



            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                seats.Add(new Seat
                {
                    SeatId = Convert.ToInt32(reader["SeatId"]),
                    BusId = Convert.ToInt32(reader["BusId"]),
                    SeatNumber = reader["SeatNumber"].ToString(),
                    IsBooked = Convert.ToBoolean(reader["IsBooked"])
                });
            }
            return seats;
        }


        public int CreateBooking(int userId, int scheduleId, int seatId)
        {
            using var connection = _dbServer.GetConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();


            try
            {

                var bookingQuery = @"
                    INSERT INTO Bookings (UserId, ScheduleId, SeatId, BookingTime,Status)
                    VALUES (@UserId, @ScheduleId, @SeatId, @BookingTime, 'Booked');
                    SELECT SCOPE_IDENTITY();";

                using var bookingCommand = new SqlCommand(bookingQuery, connection, transaction);
                bookingCommand.Parameters.AddWithValue("@UserId", userId);
                bookingCommand.Parameters.AddWithValue("@ScheduleId", scheduleId);
                bookingCommand.Parameters.AddWithValue("@SeatId", seatId);
                bookingCommand.Parameters.AddWithValue("@BookingTime", DateTime.Now);

                var bookingId = Convert.ToInt32(bookingCommand.ExecuteScalar());


                var seatQuery = "UPDATE Seats SET IsBooked = 1 WHERE SeatId = @SeatId";
                using var scheduleCommand = new SqlCommand(seatQuery, connection, transaction);    
                scheduleCommand.Parameters.AddWithValue("@SeatId", seatId); 
                scheduleCommand.ExecuteNonQuery();

                transaction.Commit();
                return bookingId;
            }
            catch
            {
                transaction.Rollback(); 
                throw;
            }
        }

        public List<Booking> GetUserBookings(int userId)
        {
            var bookings = new List<Booking>();

            using var connection = _dbServer.GetConnection();
            connection.Open();

            var query = @"
                SELECT b.BookingId, b.UserId, b.ScheduleId, b.SeatId, b.BookingTime, b.Status,u.Name as UserName,
                       s.DepartureTime, r.FromLocation, r.ToLocation,
                       st.SeatNumber
                FROM Bookings b
                INNER JOIN Users u ON b.UserId = u.UserId
                INNER JOIN Schedule s ON b.ScheduleId = s.ScheduleId
                INNER JOIN Routes r ON s.RouteId = r.RouteId
                INNER JOIN Seats st ON b.SeatId = st.SeatId
                WHERE b.UserId = @UserId
                ORDER BY b.BookingTime DESC";


            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserId", userId);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {

                bookings.Add(new Booking
                {
                    BookingId = Convert.ToInt32(reader["BookingId"]),
                    UserId = Convert.ToInt32(reader["UserId"]),
                    ScheduleId = Convert.ToInt32(reader["ScheduleId"]),
                    SeatId = Convert.ToInt32(reader["SeatId"]),
                    BookingTime = Convert.ToDateTime(reader["BookingTime"]),
                    Status = reader["Status"].ToString(),
                    UserName = reader["UserName"].ToString(),
                    FromLocation = reader["FromLocation"].ToString(),
                    ToLocation = reader["ToLocation"].ToString(),
                    DepartureTime = Convert.ToDateTime(reader["DepartureTime"]),
                    SeatNumber = reader["SeatNumber"].ToString()
                });

            }
            return bookings;
        }

        public bool CancelBooking(int bookingId, int userId)
        {
            using var connection = _dbServer.GetConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var getQuery = @" 
                    SELECT b.ScheduleId,b.SeatId
                    FROM Bookings b
                    WHERE b.BookingId = @BookingId AND b.UserId = @UserId AND b.Status = 'Booked'";


                using var getCommand = new SqlCommand(getQuery, connection, transaction);
                getCommand.Parameters.AddWithValue("@BookingId", bookingId);
                getCommand.Parameters.AddWithValue("@UserId", userId);

                using var reader = getCommand.ExecuteReader();


                if (!reader.Read())
                {
                    reader.Close();
                    transaction.Rollback();
                    return false;

                }

                var scheduleId = Convert.ToInt32(reader["ScheduleId"]);
                var seatId = Convert.ToInt32(reader["SeatId"]);
                reader.Close();


                //update booking status
                var bookingQuery = "UPDATE Bookings SET Status = 'Cancelled' WHERE BookingId = @BookingId";


                using var bookingCommand = new SqlCommand(bookingQuery, connection, transaction);
                bookingCommand.Parameters.AddWithValue("@BookingId", bookingId);
                bookingCommand.ExecuteNonQuery();



                //update seat availability
                var seatQuery = "UPDATE Seats SET IsBooked = 0 WHERE SeatId = @SeatId";
                using var seatCommand = new SqlCommand(seatQuery, connection, transaction);
                seatCommand.Parameters.AddWithValue("@SeatId", seatId);
                seatCommand.ExecuteNonQuery();


                //Update available seats
                var scheduleQuery = "UPDATE Schedule SET AvailableSeats = AvailableSeats + 1 WHERE ScheduleId = @ScheduleId";

                using var scheduleCommand = new SqlCommand(scheduleQuery, connection, transaction);
                scheduleCommand.Parameters.AddWithValue("@ScheduleId", scheduleId);
                scheduleCommand.ExecuteNonQuery();

                transaction.Commit();
                return true;

            }
            catch
            {
                transaction.Rollback();
                throw;
            }

        }
    }
}
