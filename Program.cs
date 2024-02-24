using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using BudgetTrackerHugo.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

// Login Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie();
// Comment out code below to enable Google Log in
//.AddGoogle(options =>
//{
//    options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? throw new ArgumentNullException("Authentication:Google:ClientId");
//    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? throw new ArgumentNullException("Authentication:Google:ClientSecret");
//});


// DI + Cennect with SQL DB > Azure
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));


var app = builder.Build();
//Licence key for syncfusion
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBPh8sVXJwS0R+X1pHaV1HQmFJfFdmRGlYfFR0cEUmHVdTRHRbQl9hTX9Qd0FjXXZXdXc=");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
