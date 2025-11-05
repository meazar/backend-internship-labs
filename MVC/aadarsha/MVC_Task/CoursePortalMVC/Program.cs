using Microsoft.AspNetCore.Builder; //  for  building the web application
using Microsoft.Extensions.DependencyInjection; // for adding services to the container
using Microsoft.Extensions.Hosting; // for hosting the application 
using CoursePortalMVC.Repositories; // for ICourseRepository and InMemoryCourseRepository


var builder = WebApplication.CreateBuilder(args); // Create a builder for the web application


// -- SERVICE REGISTRATION / DEPENDENCY INJECTION (DI) --
// Add MVC services (controllers + views) to the DI container.
builder.Services.AddControllersWithViews();

// Register our in-memory repository as a singleton.
// -singleton: A single instance is created and shared throughout the application's lifetime.
// This is suitable for in-memory data storage as it maintains state across requests.
builder.Services.AddSingleton<ICourseRepository, InMemoryCourseRepository>(); // Register InMemoryCourseRepository for ICourseRepository. 
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>(); // Register InMemoryUserRepository for IUserRepository.

// Add session services to enable session state management.
builder.Services.AddSession(); // Enables session management for storing user data across requests.

var app = builder.Build(); // Build the web application instance.

app.UseSession(); // Enable session middleware to manage user sessions.

// -- MIDDLEWARE PIPELINE --
// Order of middleware is important as it defines how requests are processed.
// Order matters. Each UseX is middleware that the request passes through.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Global error handling middleware
}

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS for secure communication.

// Serve static files (like CSS, JS, images) from wwwroot(css/js/images).
app.UseStaticFiles();

// Enable routing capabilities to match incoming requests to endpoints.
app.UseRouting();

// Authorization middleware to enforce security policies in the app.
// app.UseAuthorization();

// Routing: map controller routes. using the convention-based routing.
// Default route pattern(URL pattern): {controller=Course}/{action=Index}/{id?}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

// Runs the application: start the application and listen for incoming HTTP requests at the configured URL(s).
// This is a blocking call that runs the web server and processes requests.
app.Run();
