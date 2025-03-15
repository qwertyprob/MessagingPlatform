using MessagingProject.Models;
using MessagingProject.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Abstractions
{
    public interface IAuthService
    {
        Task<AuthResponseModel> AuthenticateUserAsync(LoginViewModel model);
        Task SignInUserAsync(AuthResponseModel authResponse, [FromForm] LoginViewModel model);
        Task<ErrorResponse> ResetPassword(string email);
    }

}
