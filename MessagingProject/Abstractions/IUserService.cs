using MessagingProject.Models.Auth;

namespace MessagingProject.Abstractions
{
    public interface IUserService
    {
        Task<UserClaimsInfoResponse> GetProfileInfo(string token);
        string GetToken();
        Task<bool> IsAuthenticated(string token);
        Task ChangeUILanguage(string language);
        string[] UserLoginPassword();
    }
}
