using ReportingService.Application;
using ReportingService.Persistence.Configuration;
using ReportingService.Presentanion.Configuration;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.secrets.json", optional: true, reloadOnChange: true)
            .AddCommandLine(args)
            .AddEnvironmentVariables()
            .Build();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
var configuration = builder.Configuration;
builder.Services.ConfigurePersistence(configuration);
builder.Services.ConfigureApplication(configuration);
builder.Services.ConfigurePresentation();

Log.Logger = new Serilog.LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Logging.AddSerilog();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<ExceptionMiddleware>();

app.Run();
