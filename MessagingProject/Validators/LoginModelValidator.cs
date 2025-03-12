using FluentValidation;
using MessagingProject.Models;

namespace MessagingProject.Validators
{
    public class LoginModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(MessagingProject.Resources.ValidateResource.Email);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(MessagingProject.Resources.ValidateResource.Password);
        }

    }
}
