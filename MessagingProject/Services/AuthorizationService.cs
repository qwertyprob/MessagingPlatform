using MessagingProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using static MessagingProject.Services.AuthorizationService;
using System.Security.Claims;
using System.Text;
using MessagingProject.Abstractions;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

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

            public async Task SignInUserAsync(AuthResponseModel authResponse, [FromForm] LoginViewModel model)
            {
                var claims = new List<Claim>
                {
                    new Claim("FirstName", authResponse.User.FirstName),
                    new Claim("Surname", authResponse.User.LastName),
                    new Claim("Email", model.Email),
                    new Claim("Company", authResponse.User.Company),
                    new Claim("Token", authResponse.Token),
                    new Claim("UiLanguage", authResponse?.User?.UiLanguage.ToString() ?? "0"),
                    new Claim("Password", model.Password),
                    new Claim("FullName", $"{authResponse.User.FirstName} {authResponse.User.LastName}")
                };

                var currentTokenClaim = _httpContext.User.Claims.FirstOrDefault(c => c.Type == "Token");
                if (currentTokenClaim != null)
                {
                    claims.Add(new Claim("Token", authResponse.Token));
                }
                else
                {
                    claims.Add(new Claim("Token", authResponse.Token));
                }

                var identity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(identity);

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
