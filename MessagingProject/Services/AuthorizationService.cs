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
                // Получаем текущие клеймы пользователя, если они существуют
                var claims = new List<Claim>
                {
                    new Claim("FirstName", authResponse.User.FirstName),
                    new Claim("Surname", authResponse.User.LastName),
                    new Claim(ClaimTypes.Email, authResponse.User.Email),
                };

                // Проверяем, если токен уже существует в клеймах, обновляем его
                var currentTokenClaim = _httpContext.User.Claims.FirstOrDefault(c => c.Type == "Token");
                if (currentTokenClaim != null)
                {
                    // Обновляем токен, если он уже существует
                    claims.Add(new Claim("Token", authResponse.Token));
                }
                else
                {
                    // Если токена нет, добавляем новый
                    claims.Add(new Claim("Token", authResponse.Token));
                }

                // Создаем новый ClaimsIdentity с обновленными клеймами
                var identity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(identity);

                // Подписываем пользователя с обновленными клеймами
                await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }


            private async Task RefreshToken(string token)
            {
                var urlRefreshToken = "https://dev.edi.md/ISAuthService/json/RefreshToken";

                // Отправляем запрос на обновление токена
                var response = await _client.GetAsync($"{urlRefreshToken}?Token={token}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var refreshToken = JsonConvert.DeserializeObject<AuthResponseModel>(content).Token;

                    var claims = new List<Claim>(_httpContext.User.Claims)
                    {
                        new Claim("Token", refreshToken)  // Обновляем токен
                    };

                    var identity = new ClaimsIdentity(claims, "login");

                    var principal = new ClaimsPrincipal(identity);

                    await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                }
                else
                {
                    throw new UnauthorizedAccessException("Unable to refresh token.");
                }
            }






        }


    }
}
