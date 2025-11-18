using BusSeatBookingSystem.Mains;
using BusSeatBookingSystem.Service;

class Program
{
    static void Main(string[] args)
    {
        var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=BusBookingDb;Trusted_Connection=true;TrustServerCertificate=true;";
        var dbService = new DatabaseServer(connectionString);
        var authService = new AuthService(dbService);
        var bookingService = new BookingService(dbService);
        var adminService = new Admin(dbService);

        
        var customerMenu = new CustomerMenu(authService, bookingService);
        var staffMenu = new StaffMenu(authService, adminService, bookingService);
        var adminMenu = new AdminMenu(authService, adminService, bookingService);
        var mainMenu = new MainMenu(authService, customerMenu, staffMenu, adminMenu);

        
        mainMenu.Show();

    }
}

// Initialize services






//using BusSeatBookingSystem.Service;
//using BusSeatBookingSystem.Main;
//using System;
//using Org.BouncyCastle.Bcpg.OpenPgp;
//using BusSeatBookingSystem.Model;

//public class Program
//{
//    private readonly AuthService _authService;
//    private readonly ShowcustomerMenu _showcustomerMenu;
//    private readonly ShowstaffMenucs _showstaffMenucs;
//    //private readonly BookingService _bookingService;
//    private readonly Admin _adminService;
//    private static bool _isRunning = true;

//    public Program(AuthService authService,ShowcustomerMenu showcustomerMenu,ShowstaffMenucs showstaffMenucs,BookingService bookingService,Admin AdminService)
//    {
//        _authService = authService;
//        _showcustomerMenu = showcustomerMenu;
//        _showstaffMenucs = showstaffMenucs;
//        _adminService = AdminService;


//    }

//    public static async Task Main(string[] args)
//    {
//       //InitializeService initialize = new InitializeService();

//        InitializeServices();



//        Console.WriteLine("Welcom to the Bus seat booking system!");

//        while (_isRunning)
//        {
//            if (_authService.CurrentUser == null)
//            {
//                showLoginmenu showLoginmenu = new showLoginmenu();
//                showLoginmenu.ShowLoginMenu();

//            }
//            else
//            {
//                ShowroleBasedMenu showroleBasedMenu = new ShowroleBasedMenu();
//                showroleBasedMenu.ShowRoleBasedMenu();

//            }
//        }

//    }

    //static void InitializeServices()
    //{
    //    var connectionString = "Server =(localdb)\\MSSQLLocalDB;Database=BusBookingDB;trusted_Connection=true;TrustServerCertificate=true;";
    //    var dbServer = new DatabaseServer(connectionString);
    //    _authService = new AuthService(dbServer);
    //    _bookingService = new BookingService(dbServer);
    //    _adminService = new Admin(dbServer);
    //}

//}