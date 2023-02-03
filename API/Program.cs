using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
}); // This service connects to DB using connection string supplied in the config

builder.Services.AddCors(); // Adds CORS Policy
builder.Services.AddScoped<ITokenService, TokenService>(); // Adds Token Service

var app = builder.Build();

// Middleware
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
// app.UseAuthorization(); Temporarily remove until we get to Authorization section

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
app.MapControllers();

app.Run();
