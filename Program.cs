using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using Serilog;
using Serilog.Events;
using synkrone.Data;
using synkrone.Services.Implementations;
using synkrone.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Konfigurasi logger 
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Ganti default logging provider dengan Serilog
builder.Host.UseSerilog();

// Konfigurasi Matrix
builder.Services.AddOpenTelemetry()
    .WithMetrics(matrics => matrics
        // Menambahkan sumber metrik otomatis dari ASP.NET Core
        .AddAspNetCoreInstrumentation()
        // Mengkonfigurasi exporter untuk Prometheus
        .AddPrometheusExporter());

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
    
    // Endpoint ini akan diekspos agar bisa di-scrape oleh Prometheus
    app.MapPrometheusScrapingEndpoint();

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
