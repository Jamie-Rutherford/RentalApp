using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Data;

Console.WriteLine("Running migrations...");

var context = new AppDbContext();
context.Database.Migrate();

Console.WriteLine("Migrations complete!");
