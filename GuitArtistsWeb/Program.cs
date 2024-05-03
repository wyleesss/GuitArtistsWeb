using GuitArtists.Helpers;
using FullDB.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using GuitArtists.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(AppMappingProfile));
builder.Services.AddControllersWithViews();
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connection));
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication();
builder.Services.AddTransient<IEmailSender, EmailService>();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = "GuitArtistsCookie";
    options.ExpireTimeSpan = TimeSpan.FromDays(31);
})
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientID").Value;
    options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
    options.ClaimActions.MapJsonKey("photo", "picture");
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
});
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "chords",
    pattern: "chords",
    defaults: new { controller = "Chords", action = "Search" });

app.MapControllerRoute(
    name: "articles",
    pattern: "articles",
    defaults: new { controller = "Articles", action = "Search" });

app.MapControllerRoute(
    name: "articles",
    pattern: "articles/{userLogin?}/{slug?}",
    defaults: new { controller = "Articles", action = "Index" });

app.MapControllerRoute(
    name: "login",
    pattern: "login",
    defaults: new { controller = "Login", action = "Index" });

app.MapControllerRoute(
    name: "registration",
    pattern: "registration",
    defaults: new { controller = "Registration", action = "Index" });

app.MapControllerRoute(
    name: "forgot",
    pattern: "forgot",
    defaults: new { controller = "Forgot", action = "Index" });

app.MapControllerRoute(
    name: "email-confirm",
    pattern: "email-confirm",
    defaults: new { controller = "EmailConfirmation", action = "Index" });

app.MapControllerRoute(
    name: "existing-login",
    pattern: "existing-login",
    defaults: new { controller = "ExistingLogin", action = "Index" });

app.MapControllerRoute(
    name: "create-article",
    pattern: "create-article",
    defaults: new { controller = "CreateArticle", action = "Index" });

app.MapControllerRoute(
    name: "logout",
    pattern: "logout",
    defaults: new { controller = "Logout", action = "Logout" });

app.MapControllerRoute(
    name: "recover-access",
    pattern: "recover-access/{userId?}",
    defaults: new { controller = "Recovery", action = "Index" });

app.MapControllerRoute(
    name: "profile",
    pattern: "profile/{login?}",
    defaults: new { controller = "Profile", action = "Index" });

app.MapControllerRoute(
    name: "404",
    pattern: "page-not-found",
    defaults: new { controller = "PageNotFound", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseStatusCodePagesWithReExecute("/PageNotFound");

app.UseSession();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Data", "Images")),
    RequestPath = "/Data/Images"
});

app.Run();