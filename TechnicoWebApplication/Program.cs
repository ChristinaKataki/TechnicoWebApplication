using Microsoft.EntityFrameworkCore;
using TechnicoWebApplication.Context;
using TechnicoWebApplication.Repositories;
using TechnicoWebApplication.Services;

var builder = WebApplication.CreateBuilder(args);

// Enable detailed logs in development mode
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Log to the console
builder.Logging.AddDebug();   // Log to debug output (optional)


// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddDbContext<TechnicoDbContext>();
builder.Services.AddScoped<PropertyOwnerService>();
builder.Services.AddScoped<PropertyOwnerRepository>();
builder.Services.AddScoped<PropertyItemService>();
builder.Services.AddScoped<PropertyItemRepository>();
builder.Services.AddScoped<RepairService>();
builder.Services.AddScoped<RepairRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
