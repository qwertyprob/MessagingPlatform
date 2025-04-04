﻿using MessagingProject.Abstractions;
using MessagingProject.Services;
using System.Runtime.CompilerServices;

namespace MessagingProject.DIContainer
{
    public static class AuthServices
    {
        public static void AddAuthServices(this IServiceCollection services)
        {
            services.AddSession();
            services.AddAuthentication("Cookies")
                            .AddCookie("Cookies", options =>
                            {
                                options.LoginPath = "/Login";
                                options.LogoutPath = "/Logout";
                                options.Cookie.HttpOnly = true;
                                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                                //options.ExpireTimeSpan = TimeSpan.FromSeconds(5);
                                options.SlidingExpiration = true;
                            });

            services.AddAuthorization();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
