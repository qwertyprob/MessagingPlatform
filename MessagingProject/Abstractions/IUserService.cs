using MessagingProject.Models;

namespace MessagingProject.Abstractions
{
    public interface IUserService
    {
        Task<AuthResponseModel> GetProfileInfo(string token);
        Task<bool> IsAuthenticated(string token);
    }
}
