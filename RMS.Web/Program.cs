using Microsoft.AspNetCore.Authentication.Cookies;
using RMS.Data.Services;
using RMS.Web;

// NOTE: Changes needed here to enable Authentication Stretch Requirement
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => {
            options.AccessDeniedPath= "/User/ErrorNotAuthorised";
            options.LoginPath = "/User/ErrorNotAuthenticated";
        });  
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
} else {
    // in development mode seed the database each time the application starts
    ServiceSeeder.SeedUsers(new UserServiceDb());
    ServiceSeeder.SeedRecipes(new RecipeServiceDb());
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

