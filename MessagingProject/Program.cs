using MessagingProject.Abstractions;
using MessagingProject.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using static MessagingProject.Services.AuthorizationService;

namespace MessagingProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication("Cookies")
                            .AddCookie("Cookies", options =>
                            {
                                options.LoginPath = "/Login";
                                options.LogoutPath = "/Logout";
                                options.Cookie.HttpOnly = true;
                                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                            });



            builder.Services.AddAuthorization();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();

            //HttpClientFactory 

            builder.Services.AddHttpClient("AuthLogin", client =>
            {
                client.BaseAddress = new Uri("https://dev.edi.md/ISAuthService/json/AuthorizeUser");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });


            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Dashboard/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            

            app.UseAuthentication();
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
