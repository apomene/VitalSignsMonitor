using Microsoft.EntityFrameworkCore;
using VitalSignsMonitor.Models;
using VitalSignsMonitor.Hubs;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();


// Configure EF Core with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=vitals.db"));

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();
app.MapHub<VitalSignsHub>("/vitalsHub");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
