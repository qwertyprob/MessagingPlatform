using MessagingProject.Abstractions;
using MessagingProject.Models;
using Newtonsoft.Json;

namespace MessagingProject.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;
        public UserService(HttpClient client)
        {
            _client = client;

        }

        public async Task<AuthResponseModel> GetProfileInfo(string token)
        {

            _client.BaseAddress = new Uri("https://dev.edi.md/ISAuthService/json/GetProfileInfo");
            var response = _client.GetAsync(_client.BaseAddress + $"?Token={token}").Result;

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AuthResponseModel>(content);
            }
            return null;
        }

        public async Task<bool> IsAuthenticated(string token)
        {
            var profileInfo = await GetProfileInfo(token);

            return profileInfo != null && !string.IsNullOrEmpty(profileInfo.Token);
        }

    }
}
