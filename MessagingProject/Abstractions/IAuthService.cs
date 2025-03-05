using MessagingProject.Models;

namespace MessagingProject.Abstractions
{
    public interface IAuthService
    {
        Task<AuthResponseModel> AuthenticateUserAsync(LoginViewModel model);
        Task SignInUserAsync(AuthResponseModel authResponse);
        
    }

}
