using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
}); // This service connects to DB using connection string supplied in the config

var app = builder.Build();

// Middleware
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// app.UseAuthorization(); Temporarily remove until we get to Authorization section

app.MapControllers();

app.Run();
