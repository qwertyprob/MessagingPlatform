using FluentValidation;
using FluentValidation.AspNetCore;
using MessagingProject.Abstractions;
using MessagingProject.DIContainer;
using MessagingProject.Services;
using MessagingProject.Validators;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;

namespace MessagingProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews(options =>
            {
                //Clear all system exceptions instead of fluent validation
                options.ModelMetadataDetailsProviders.Clear(); 

            })
                   .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            //Validation Container
            builder.Services.AddValditaion();

            //Auth Service Container
            builder.Services.AddAuthServices();

            //Contact Service Container
            builder.Services.AddContactServices();
            //Decrypt Service Container
            builder.Services.AddDecryptServices();

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            //HttpClientFactory 
            builder.Services.AddHttpServices(builder.Configuration);

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Dashboard/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            //Globalization
            app.Use(async (context, next) =>
            {
                string cookie = string.Empty;
                if(context.Request.Cookies.TryGetValue("Language",out cookie))
                {
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cookie);
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cookie);
                }
                else
                {
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en");
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                }

                await next.Invoke();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            //Routing
            app.MapControllerRoute(
                name: "MainPage",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "Contacts",
                pattern: "{controller=Contacts}/{action=ContactLists}"
            );

            app.UseEndpoints(endpoints =>
            {
                endpoints?.MapControllers().RequireAuthorization();
            });

            app.Run();
        }
    }
}
