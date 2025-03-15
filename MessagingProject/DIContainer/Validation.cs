using FluentValidation;
using MessagingProject.Validators;

namespace MessagingProject.DIContainer
{
    public static class Validation 
    {
        public static void AddValditaion(this IServiceCollection services)
        {

            services.AddValidatorsFromAssemblyContaining<LoginModelValidator>();

        }
    }
}
