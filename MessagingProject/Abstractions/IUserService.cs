using MessagingProject.Models;

namespace MessagingProject.Abstractions
{
    public interface IUserService
    {
        Task<UserClaimsInfoResponse> GetProfileInfo(string token);
        Task<string> GetToken();
        Task<bool> IsAuthenticated(string token);
    }
}
