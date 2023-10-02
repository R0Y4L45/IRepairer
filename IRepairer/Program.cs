using App.Business.Abstract;
using App.Business.Concrete;
using App.Entities.DbCon;
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
        builder.Services.AddDbContext<IRepairerDbContext>(_ => _.UseSqlServer(builder.Configuration["ConnectionStrings:default"]));
        //Services register
        builder.Services.AddSingleton<IRatingService, RatingService>();
        builder.Services.AddSingleton<ICategoryService, CategoryService>();
        builder.Services.AddSingleton<IRepairerService, RepairerService>();

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