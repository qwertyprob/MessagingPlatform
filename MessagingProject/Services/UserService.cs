using MessagingProject.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MessagingProject.Models.Auth;
using System.Threading.Tasks;

namespace MessagingProject.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<UserClaimsInfoResponse> GetProfileInfo(string token)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                return Task.FromResult<UserClaimsInfoResponse>(null);
            }

            var claims = user.Claims;
            var firstName = claims.FirstOrDefault(c => c.Type == "FirstName")?.Value ?? string.Empty;
            var lastName = claims.FirstOrDefault(c => c.Type == "Surname")?.Value ?? string.Empty;

            var userInfo = new UserClaimsInfoResponse
            {
                Company = claims.FirstOrDefault(c => c.Type == "Company")?.Value ?? string.Empty,
                Email = claims.FirstOrDefault(c => c.Type == "Email")?.Value ?? string.Empty,
                FullName = $"{lastName} {firstName}".Trim(),
                UiLanguage = GetLanguage(),
                Password = claims.FirstOrDefault(c => c.Type == "Password")?.Value ?? string.Empty,
                Token = token
            };

            return Task.FromResult(userInfo);
        }

        public string GetToken()
        {
            var claims = _httpContextAccessor.HttpContext?.User.Claims;
            var token = claims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? string.Empty;


            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }

            return string.Empty;
        }


        private string GetLanguage()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies["Language"] ?? "en";
        }

        public Task ChangeUILanguage(string language)
        {
            if (!string.IsNullOrEmpty(language))
            {
                _httpContextAccessor.HttpContext?.Response.Cookies.Append("Language", language, new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddYears(1)
                });
            }
            return Task.CompletedTask;
        }
    }
}
