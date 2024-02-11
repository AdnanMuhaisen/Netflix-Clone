using Microsoft.EntityFrameworkCore;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Serilog;
using Netflix_Clone.API.Extensions;
using Netflix_Clone.Domain;
using Netflix_Clone.Application.Services.FileOperations;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//configure the options:
builder.Services.Configure<ContentOptions>(builder.Configuration.GetSection("Content:Movies"));


//app services:

//configure the logger
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSerilog();
});

//register db context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//configure the mediator:
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
});

builder.Services.RegisterMapsterConfigurations();


builder.Services.AddScoped<IFileCompressor, FileCompressor>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
