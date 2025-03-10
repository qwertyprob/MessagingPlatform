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
using System.Reflection;

namespace MessagingProject.Services
{
    public class AuthorizationService
    {
        public class AuthService : IAuthService
        {
            private readonly HttpClient _client;
            private readonly IUserService _userService;
            private readonly HttpContext _httpContext;

            public AuthService(HttpClient client,IHttpContextAccessor context,IUserService userService)
            {
                _client = client;
                _userService = userService;
                _client.BaseAddress = new Uri("https://dev.edi.md/ISAuthService/json/");
                _httpContext = context.HttpContext;
            }
            
            public async Task<AuthResponseModel> AuthenticateUserAsync(LoginViewModel model)
            {
                var requestContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("AuthorizeUser", requestContent);

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
                

                var identity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(identity);

                await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }



            public async Task<ErrorResponse> ResetPassword(string email)
            {
                if (string.IsNullOrEmpty(email))
                {
                    throw new ArgumentException("Email cannot be null or empty", nameof(email));
                }
                var requestData = new { Email = email }; // Создаем объект
                var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("ResetPassword", requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Server error: {response.StatusCode}. Details: {errorMessage}");
                }

                var content = await response.Content.ReadAsStringAsync();

                var responseContent = JsonConvert.DeserializeObject<ErrorResponse>(content);

                if (responseContent == null || string.IsNullOrEmpty(responseContent.ErrorMessage))
                {
                    throw new UnauthorizedAccessException("Invalid response content or missing error message.");
                }

                return responseContent;
            }







        }


    }
}
