using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;

namespace MessagingProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Dashboard/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "MainPage",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "Contacts",
                pattern: "{controller=Contacts}/{action=ContactLists}"
            );

            













            app.Run();
        }
    }
}
