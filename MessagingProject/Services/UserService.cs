using MessagingProject.Abstractions;
using MessagingProject.Models;

namespace MessagingProject.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;
        public UserService(HttpClient client)
        {
            _client = client;
        }
    }
}
