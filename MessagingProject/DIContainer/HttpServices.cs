using MessagingProject.Abstractions;
using MessagingProject.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingProject.DIContainer
{
    public static class HttpServices
    {
        public static void AddHttpServices(this IServiceCollection services, IConfiguration Config)
        {
            //HttpContext
            services.AddHttpContextAccessor();

            //Auth
            services.AddHttpClient<IAuthService, AuthService>(options =>
            {
#if DEBUG
                options.BaseAddress = new Uri(Config["ApiUrls:Auth"]);
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
                options.BaseAddress = new Uri(Config["ApiUrls:Contact"]);
#endif
#if RELEASE
                options.BaseAddress = new Uri("https://dev.edi.md/MailService/");
#endif

                options.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            //Email
            services.AddHttpClient<IEmailService,EmailService>(options =>
            {
#if DEBUG
                options.BaseAddress = new Uri(Config["ApiUrls:Email"]);
#endif
#if RELEASE
                options.BaseAddress = new Uri(Config["ApiUrls:Email"]);
#endif

                options.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            //Sms
            services.AddHttpClient<ISMSService,SMSService>(options =>
            {
#if DEBUG
                options.BaseAddress = new Uri(Config["ApiUrls:Sms"]);
#endif
#if RELEASE
                options.BaseAddress = new Uri(Config["ApiUrls:Sms"]);
#endif

                options.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}
