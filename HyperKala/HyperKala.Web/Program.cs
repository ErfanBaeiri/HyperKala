using GoogleReCaptcha.V3.Interface;
using GoogleReCaptcha.V3;
using HyperKala.DataLayer.Context;
using HyperKala.IoC;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Configuration ConnectionString
builder.Services.AddDbContext<HyperKalaDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("HyperKalaCS")));
#endregion

#region Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Login";
    options.LogoutPath = "/LogOut";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
});
#endregion

#region Registr Dependency
DependencyContainer.RegisterDependencies(builder.Services);
//Google Recaptcha.V3
builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
#endregion

#region Encode for support Persian language 
builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
#endregion

#region Middleware
var app = builder.Build();

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

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();





app.Run();
#endregion

