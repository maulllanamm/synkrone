using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using synkrone.Data;
using synkrone.Services.Implementations;
using synkrone.Services.Interfaces;

// Konfigurasi logger utama sebelum build
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // Level log minimum global
    .Enrich.FromLogContext()
    .WriteTo.Console() // 🪵 Sink #1: Menulis log ke konsol
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Ganti default logging provider dengan Serilog
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Get connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 2. Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
        .UseSnakeCaseNamingConvention());

builder.Services.AddScoped<IUserService, UserService>();

try
{
    Log.Information("Starting up the application");

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start correctly");
}
finally
{
    Log.CloseAndFlush();
}
