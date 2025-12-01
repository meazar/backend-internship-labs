using System.Reflection;
using System.Text;
using KycManagementSystem.Api.Database;
using KycManagementSystem.Api.Repositories.Implementations;
using KycManagementSystem.Api.Repositories.Interfaces;
using KycManagementSystem.Api.Services.Implementations;
using KycManagementSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;  // Comes from Swashbuckle.AspNetCore

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// Configuration helpers
// -------------------------
var jwtSection = builder.Configuration.GetSection("Jwt");
if (string.IsNullOrEmpty(jwtSection["Key"]))
{
    throw new InvalidOperationException("JWT configuration is missing. Ensure 'Jwt:Key' is set in configuration.");
}
var key = Encoding.UTF8.GetBytes(jwtSection["Key"]);

// -------------------------
// Add services to DI (use correct lifetimes)
// -------------------------
builder.Services.AddHttpContextAccessor();

// Infrastructure
builder.Services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

// Repositories (Scoped)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IKycRepository, KycRepository>();
builder.Services.AddScoped<IOfacRepository, OfacRepository>();
builder.Services.AddScoped<ILogsRepository, LogsRepository>();
builder.Services.AddScoped<IDocumentsRepository, DocumentsRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IKycHistoryRepository, KycHistoryRepository>();

// Services (Scoped)
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IKycService, KycService>();
builder.Services.AddScoped<IOfacService, OfacService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IKycHistoryService, KycHistoryService>();
// ----------------------------
// AUTHENTICATION (FINAL FIX)
// ----------------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


// Controllers + filters registered by type (so filters resolve per-request)
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

// Swagger with JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "KYC", Version = "v1" });
    // x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    x.AddSecurityDefinition("Bearer", securityScheme);

    x.AddSecurityRequirement(new OpenApiSecurityRequirement// Make sure Swagger UI requires a Bearer token to be passed
     {
         {
             securityScheme,
             new string[] {}
         }
     });
    x.CustomSchemaIds(type => type.FullName);
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Middlewares
// RequestLoggingMiddleware must not require scoped services in constructor.
// It will resolve ILogsRepository per-request from HttpContext.RequestServices.
app.UseMiddleware<RequestLoggingMiddleware>();
// RateLimitingMiddleware uses only primitives / request delegate -> safe
app.UseMiddleware<RateLimitingMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
