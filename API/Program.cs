using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
using API.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);

// Snippet
var connString = "";
if (builder.Environment.IsDevelopment()) 
    connString = builder.Configuration.GetConnectionString("DefaultConnection");
else 
{
    // Use connection string provided at runtime by FlyIO.
    var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    // Parse connection URL to connection string for Npgsql
    if (connUrl != null)
    {
        connUrl = connUrl.Replace("postgres://", string.Empty);
        var pgUserPass = connUrl.Split("@")[0];
        var pgHostPortDb = connUrl.Split("@")[1];
        var pgHostPort = pgHostPortDb.Split("/")[0];
        var pgDb = pgHostPortDb.Split("/")[1];
        var pgUser = pgUserPass.Split(":")[0];
        var pgPass = pgUserPass.Split(":")[1];
        var pgHost = pgHostPort.Split(":")[0];
        var pgPort = pgHostPort.Split(":")[1];
        var updatedHost = pgHost.Replace("flycast", "internal");

        // Check potential spacing if errors occur
        connString = $"Server={updatedHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";
    }

    //"Server=host.docker.internal; Port=5432; User Id=postgres; Password=postgrespw; Database=datingapp"
}
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseNpgsql(connString);
}); // This service connects to DB using connection string supplied in the config
// End snippet

var app = builder.Build();

// Middleware
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
// app.UseAuthorization(); Temporarily remove until we get to Authorization section

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials() 
    .WithOrigins("https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles(); // Gets index.html from wwwroot 
app.UseStaticFiles(); // Looks for wwwroot folder and serves content from there
    
app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
app.MapFallbackToController("Index", "Fallback");

// Database migrations in code
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await Seed.ClearConnections(context);
    // await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Connections\""); // Bigger scale databases
    // context.Connections.RemoveRange(context.Connections); // Good for small scale remove operations
    await Seed.SeedUsers(userManager, roleManager);
}
catch (Exception e)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(e, "An error occurred during migration");
}

app.Run();
