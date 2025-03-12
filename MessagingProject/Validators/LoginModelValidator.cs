using FluentValidation;
using MessagingProject.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace MessagingProject.Validators
{
    public class LoginModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(MessagingProject.Resources.ValidateResource.Email)
                .NotNull().WithMessage(MessagingProject.Resources.ValidateResource.Email)
                .EmailAddress().WithMessage(MessagingProject.Resources.ValidateResource.Email)
                .WithMessage(MessagingProject.Resources.ValidateResource.Email);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(MessagingProject.Resources.ValidateResource.Password)
                .NotNull()
                .WithMessage(MessagingProject.Resources.ValidateResource.Password);
        }

    }
}
