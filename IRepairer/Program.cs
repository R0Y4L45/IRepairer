using App.Business.Abstract;
using App.Business.Concrete;
using App.Entities.DbCon;
using App.Entities.Entity;
using IRepairer.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IRepairer;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        //Db Add
        string conn = builder.Configuration["ConnectionStrings:default"];
        builder.Services.AddDbContext<CustomIdentityDbContext>(_ => _.UseSqlServer(conn));

        builder.Services.AddIdentity<CustomIdentityUser, CustomIdentityRole>(_ => _.Password.RequiredLength = 5)
        .AddEntityFrameworkStores<CustomIdentityDbContext>().AddDefaultTokenProviders();

        //Services register
        builder.Services.AddSingleton<ICategoryService, CategoryService>();
        builder.Services.AddSingleton<IRepairerService, RepairerService>();
        builder.Services.AddSignalR();

        builder.Services.AddAuthentication();

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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHub<ChatHub>("/chatHub");

        app.MapControllerRoute(
            name: "userArea",
            pattern: "User/{controller=User}/{action=Main}/{id?}",
            defaults: new { area = "User" }
            );

        app.MapControllerRoute(
            name: "adminArea",
            pattern: "Admin/{controller=Admin}/{action=Main}/{id?}",
            defaults: new { area = "Admin" }
            );

        app.MapControllerRoute(
            name: "repairerArea",
            pattern: "Repairer/{controller=Repairer}/{action=Main}/{id?}",
            defaults: new { area = "Repairer" }
            );

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=LogIn}/{id?}"
            );

        app.Run();
    }
}