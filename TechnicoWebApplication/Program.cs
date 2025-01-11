using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TechnicoWebApplication.Context;
using TechnicoWebApplication.Repositories;
using TechnicoWebApplication.Services;
using TechnicoWebApplication.Validators;

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

builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

builder.Services.AddDbContext<TechnicoDbContext>();
builder.Services.AddScoped<PropertyOwnerService>();
builder.Services.AddScoped<PropertyOwnerRepository>();
builder.Services.AddScoped<PropertyItemService>();
builder.Services.AddScoped<PropertyItemRepository>();
builder.Services.AddScoped<RepairService>();
builder.Services.AddScoped<RepairRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<OwnerValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<OwnerFiltersValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PropertyItemValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ItemFiltersValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RepairValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RepairFiltersValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Add security definition for JWT Bearer Token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter your JWT Bearer token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

//cors
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
});


// JWT configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Technico",
            ValidAudience = "Technico",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SomeLongSecretKeyForSigningThatIs256BitsLong"))
        };
        options.Events = new JwtBearerEvents
        {
            OnForbidden = async context =>
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Forbidden: You do not have permission to access this resource.");
            }
        };
    });

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors(MyAllowSpecificOrigins);

app.Run();
