using MessagingProject.Abstractions;
using MessagingProject.Services;

namespace MessagingProject.DIContainer
{
    public static class ContactServices
    {
        public static void AddContactServices(this IServiceCollection services)
        {
            services.AddScoped<IContactService, ContactService>();
        }
    }
}
