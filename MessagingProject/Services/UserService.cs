using MessagingProject.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Globalization;
using MessagingProject.Models.Auth;

namespace MessagingProject.Services
{
    public class UserService : IUserService
    {

        private readonly HttpContext _httpContext;
        public UserService(IHttpContextAccessor httpContext)
        {
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
                Company = claims.FirstOrDefault(c => c.Type == "Company")?.Value ?? string.Empty,
                Email = claims.FirstOrDefault(c => c.Type == "Email")?.Value ?? string.Empty,
                FullName = $"{LastName} {FirstName}",
                UiLanguage = claims.FirstOrDefault(c => c.Type == "UiLanguage")?.Value ?? string.Empty,
                Password = claims.FirstOrDefault(c => c.Type == "Password")?.Value ?? string.Empty,
                Token = token
            };

            userInfo.UiLanguage = SetLanguage();
            UpdateUiLanguageClaims(userInfo.UiLanguage);

            // Возвращаем обновленные данные пользователя
            return Task.FromResult(userInfo);
        }
        private string SetLanguage()
        {
            var uiLanguage = _httpContext.Request.Cookies["Language"];
            return uiLanguage??"en";
        }
        public Task ChangeUILanguage(string language)
        {
           if(!string.IsNullOrEmpty(language))
           {
                _httpContext.Response.Cookies.Append("Language", language, new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddYears(1)
                });
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
           }


            return Task.CompletedTask;
        }
        private async void UpdateUiLanguageClaims(string uiLanguage)
        {
            var user = _httpContext.User;
            var claims = user.Claims.ToList();

            var uiLanguageClaim = claims.FirstOrDefault(c => c.Type == "UiLanguage");
            if (uiLanguageClaim != null)
            {
                claims.Remove(uiLanguageClaim);
            }

            claims.Add(new Claim("UiLanguage", uiLanguage));

            var identity = new ClaimsIdentity(claims, "login");
            var newUser = new ClaimsPrincipal(identity);

            _httpContext.User = newUser;

            // Обновляем куки с актуальными данными
            await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, newUser);
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

            return string.Empty;

        }
        public string[] UserLoginPassword()
        {
            var user = _httpContext.User;
            var claims = user.Claims;

            var password = claims.FirstOrDefault(c => c.Type == "Password")?.Value;
            var email = claims.FirstOrDefault(c => c.Type == "Email")?.Value;

            var array = new string[]{ email, password };


            return array;

        }
        public async Task<bool> IsAuthenticated(string token)
        {
            var profileInfo =  await GetProfileInfo(token);

            return profileInfo != null && !string.IsNullOrEmpty(profileInfo.Token);
        }

    }
}
