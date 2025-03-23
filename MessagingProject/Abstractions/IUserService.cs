using MessagingProject.Models.Auth;

namespace MessagingProject.Abstractions
{
    public interface IUserService
    {
        Task<UserClaimsInfoResponse> GetProfileInfo(string token);
        string GetToken();
        Task ChangeUILanguage(string language);
    }
}
