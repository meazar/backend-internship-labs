using BusSeatBookingSystem.Model;

using BusSeatBookingSystem.Service;

namespace BusSeatBookingSystem.Mains
{
    public class MainMenu
    {
        private readonly AuthService _authService;
        private readonly CustomerMenu _customerMenu;
        private readonly StaffMenu _staffMenu;
        private readonly AdminMenu _adminMenu;
        private bool _isRunning = true;

        public MainMenu(AuthService authService, CustomerMenu customerMenu, StaffMenu staffMenu, AdminMenu adminMenu)
        {
            _authService = authService;
            _customerMenu = customerMenu;
            _staffMenu = staffMenu;
            _adminMenu = adminMenu;
        }

        public void Show()
        {


            Console.WriteLine("=== Bus Seat Booking System ===");

            while (_isRunning)
            {
                if (_authService.CurrentUser == null)
                {
                    ShowLoginMenu();
                }
                else
                {
                    ShowRoleBasedMenu();
                }
            }
        }

        private void ShowLoginMenu()
        {
            //string password = "Staff_123";
            //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            //Console.WriteLine($"\n[Debug] Hashed Admin Password: {hashedPassword}");


            Console.WriteLine("\n1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Login();
                    break;
                case "2":
                    Register();
                    break;
                case "3":
                    _isRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid option!");
                    break;
            }
        }

        private void Login()
        {
            Console.Write("Email: ");
            var email = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();

            if (_authService.Login(email, password))
            {
                Console.WriteLine($"\nWelcome {_authService.CurrentUser.Name} ({_authService.CurrentUser.Role})!");
            }
            else
            {
                Console.WriteLine("Invalid credentials!");
            }
        }

        private void Register()
        {
            Console.Write("Name: ");
            var name = Console.ReadLine();
            Console.Write("Email: ");
            var email = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();
            Console.Write("Role:");
            var role = Console.ReadLine();

            try
            {
                _authService.Register(name, email, password, role);
                Console.WriteLine("Registration successful! Please login.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration failed: {ex.Message}");
            }
        }

        private void ShowRoleBasedMenu()
        {
            switch (_authService.CurrentUser.Role)
            {
                case "Admin":
                    _adminMenu.Show();
                    break;
                case "Staff":
                    _staffMenu.Show();
                    break;
                case "Customer":
                    _customerMenu.Show();
                    break;
            }

            // If we return from role menu, user logged out
            if (_authService.CurrentUser == null)
            {
                Console.WriteLine("Logged out successfully!");
            }
        }
    }
}