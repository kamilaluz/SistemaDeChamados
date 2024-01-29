using Microsoft.EntityFrameworkCore;
using Tickets.Services;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using Tickets.Models;

namespace Tickets
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add services to database
            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddDefaultIdentity<Usuario>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDbContext>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "chamados",
                pattern: "Chamados/Index/Id",
                defaults: new { controller = "Chamados", action = "Index" }
            );

            app.MapControllerRoute(
                name: "usuarios",
                pattern: "Usuarios/Index",
                defaults: new { controller = "Usuarios", action = "Index" }
            );

            app.MapRazorPages();

            app.Run();
        }
    }
}
