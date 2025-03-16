using MessagingProject.Abstractions;
using MessagingProject.Services;

namespace MessagingProject.DIContainer
{
    public static class HttpServices
    {
        public static void AddHttpServices(this IServiceCollection services)
        {
            //HttpContext
            services.AddHttpContextAccessor();

            //Auth
            services.AddHttpClient<IAuthService, AuthService>(options =>
            {
#if DEBUG
                options.BaseAddress = new Uri("https://dev.edi.md/ISAuthService/json/");
#endif
#if RELEASE
                options.BaseAddress = new Uri("https://dev.edi.md/ISAuthService/json/");
#endif
                options.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            //Contact
            services.AddHttpClient<IContactService, ContactService>(options =>
            {
#if DEBUG
                options.BaseAddress = new Uri("https://dev.edi.md/MailService/");
#endif
#if RELEASE
                options.BaseAddress = new Uri("https://dev.edi.md/MailService/");
#endif

                options.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}
