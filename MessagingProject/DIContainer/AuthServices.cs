using MessagingProject.Abstractions;
using MessagingProject.Services;
using System.Runtime.CompilerServices;
using static MessagingProject.Services.AuthorizationService;

namespace MessagingProject.DIContainer
{
    public static class AuthServices
    {
        public static void AddAuthServices(this IServiceCollection services)
        {
            services.AddAuthentication("Cookies")
                            .AddCookie("Cookies", options =>
                            {
                                options.LoginPath = "/Login";
                                options.LogoutPath = "/Logout";
                                options.AccessDeniedPath = "/Login";
                                options.Cookie.HttpOnly = true;
                                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;


                            });

            services.AddAuthorization();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
