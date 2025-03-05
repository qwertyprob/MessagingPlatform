using MessagingProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using static MessagingProject.Services.AuthorizationService;
using System.Security.Claims;
using System.Text;
using MessagingProject.Abstractions;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace MessagingProject.Services
{
    public class AuthorizationService
    {
        public class AuthService : IAuthService
        {
            private readonly HttpClient _client;
            private readonly HttpContext _httpContext;

            public AuthService(HttpClient client,IHttpContextAccessor context)
            {
                _client = client;
                _client.BaseAddress = new Uri("https://dev.edi.md/ISAuthService/json/AuthorizeUser");
                _httpContext = context.HttpContext;
            }
            
            public async Task<AuthResponseModel> AuthenticateUserAsync(LoginViewModel model)
            {
                var requestContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(_client.BaseAddress, requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Invalid credentials or server error.");
                }

                var content = await response.Content.ReadAsStringAsync();
                var authResponse = JsonConvert.DeserializeObject<AuthResponseModel>(content);

                if (authResponse == null || string.IsNullOrEmpty(authResponse.Token))
                {
                    throw new UnauthorizedAccessException("Invalid token.");
                }

                return authResponse;
            }

            public async Task SignInUserAsync(AuthResponseModel authResponse)
            {
                var claims = new List<Claim>
                {
                    new Claim("FirstName", authResponse.User.FirstName),
                    new Claim("Surname", authResponse.User.LastName),
                    new Claim(ClaimTypes.Email, authResponse.User.Email),
                    new Claim("Token", authResponse.Token)
                };

                var identity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(identity);

                // Авторизуем пользователя
                await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }

            



        }


    }
}
