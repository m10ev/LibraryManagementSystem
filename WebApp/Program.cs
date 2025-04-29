using Data;
using Microsoft.EntityFrameworkCore;
using WebApp.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the DbContext with the connection string
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the DbSeeder class for dependency injection
builder.Services.AddScoped<DbSeeder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// Seed the database on application startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    var dbSeeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();

    // Check if the app is running in Development environment
    if (app.Environment.IsDevelopment())
    {
        // Check if the database is empty (check if key tables have any records)
        if (!context.Authors.Any() && !context.Members.Any() && !context.Books.Any() && !context.BorrowedBooks.Any())
        {
            // Seed the database if any table is empty
            await dbSeeder.Seed();
        }
    }
}

app.MapStaticAssets(); // Assuming you have a method like this for static assets

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
