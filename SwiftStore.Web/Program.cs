using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Swift.DataLayer.Models;

var builder = WebApplication.CreateBuilder(args);


#region Services
// Add services to the container.
builder.Services.AddControllersWithViews();
#endregion

#region Connection String
builder.Services.AddDbContext<SwiftStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SwiftConnection")));
#endregion

#region MiddlWare
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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
#endregion

