using Infrastructure.Persistence;

using Infrastructure;
using Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Application;
using Microsoft.Extensions.DependencyInjection; 
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();


builder.Services.AddInfrastructure(builder.Configuration);


var app = builder.Build();

// --- Seed the database ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // Apply any pending migrations
        context.Database.Migrate();
        // Seed the data
        DbInitializer.Seed(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the DB.");
    }
}
// -------------------------

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