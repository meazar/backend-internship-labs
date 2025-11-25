
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Options;
using TeacherAPI.Repositories;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<InMemoryTeacherRepository>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //options.JsonSerializerOptions.PropertyNameCaseInsensitive = true; // Enforce case sensitivity
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.Strict; // Strict number handling (rejects "4" for int fields)
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; // Ignore null values in responses
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<StrictJsonMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
