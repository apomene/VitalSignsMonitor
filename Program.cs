using Microsoft.EntityFrameworkCore;
using VitalSignsMonitor.Models;
using VitalSignsMonitor.Hubs;
using Microsoft.AspNetCore.Identity;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() 
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; 
});


builder.Services.AddControllersWithViews();


// Configure EF Core with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=vitals.db"));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

var app = builder.Build();
app.MapHub<VitalSignsHub>("/vitalsHub");


async Task SeedRoles(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Admin", "Doctor", "Nurse" };

    foreach (var role in roles)
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    //await SeedRoles(services);
    await DbInitializer.SeedRolesAndAdminAsync(services);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages(); 


app.MapStaticAssets();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Patient}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
