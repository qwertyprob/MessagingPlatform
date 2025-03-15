using MessagingProject.Abstractions;
using MessagingProject.Services;

namespace MessagingProject.DIContainer
{
    public static class DecrytpService
    {
        public static void AddDecryptServices(this IServiceCollection services)
        {
            services.AddSingleton<IDecryptor, Decryptor>();
        }
    }
}
