using MessagingProject.Abstractions;
using MessagingProject.Models;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MessagingProject.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;
        private readonly HttpContext _httpContext;
        public UserService(HttpClient client,IHttpContextAccessor httpContext)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://dev.edi.md/ISAuthService/json/GetProfileInfo");
            _httpContext = httpContext.HttpContext;

        }

        public Task<UserClaimsInfoResponse> GetProfileInfo(string token)
        {

            var user = _httpContext.User;
            var claims = user.Claims;
            var FirstName = claims.FirstOrDefault(c => c.Type == "FirstName")?.Value;
            var LastName = claims.FirstOrDefault(c => c.Type == "Surname")?.Value;

            var userInfo = new UserClaimsInfoResponse
            {
                Company = claims.FirstOrDefault(c => c.Type == "Company")?.Value,
                Email = claims.FirstOrDefault(c => c.Type == "Email")?.Value,
                FullName = $"{FirstName} {LastName}",
                UiLanguage = claims.FirstOrDefault(c => c.Type == "UiLanguage")?.Value,
                Password = claims.FirstOrDefault(c => c.Type == "Password")?.Value,
                Token = token

            };
            userInfo.UiLanguage = SetLanguage(userInfo.UiLanguage);




            return Task.FromResult(userInfo);
        }
        private string SetLanguage(string uiLanguage)
        {
            switch (uiLanguage)
            {
                case "1":
                    uiLanguage = "ro";  
                    break;

                case "2":
                    uiLanguage = "ru";  
                    break;

                case "0":
                    uiLanguage = "en";  
                    break;

                default:
                    uiLanguage = "en";  
                    break;
            }

            var user = _httpContext.User;
            var claims = user.Claims.ToList();

            var uiLanguageClaim = claims.FirstOrDefault(c => c.Type == "UiLanguage");
            if (uiLanguageClaim != null)
            {
                claims.Remove(uiLanguageClaim);
            }

            claims.Add(new Claim("UiLanguage", uiLanguage));

            var identity = new ClaimsIdentity(claims, "custom"); // "custom" — тип аутентификации
            var newUser = new ClaimsPrincipal(identity);

            _httpContext.User = newUser;

            return uiLanguage;
        }

        public string GetToken()
        {

            var user = _httpContext.User;
            var claims = user.Claims;

            var token = claims.FirstOrDefault(c => c.Type == "Token")?.Value;


            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }

            return null;

        }

        public async Task<bool> IsAuthenticated(string token)
        {
            var profileInfo =  await GetProfileInfo(token);

            return profileInfo != null && !string.IsNullOrEmpty(profileInfo.Token);
        }

    }
}
