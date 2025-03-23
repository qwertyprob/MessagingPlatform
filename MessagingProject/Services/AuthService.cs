using MessagingProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using MessagingProject.Abstractions;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MessagingProject.Models.Auth;
using static DevExpress.Data.Helpers.FindSearchRichParser;
using System.Net;

namespace MessagingProject.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _client;
        private readonly HttpContext _httpContext;
        public AuthService(HttpClient client, IHttpContextAccessor context)
        {
            _client = client;
            _httpContext = context.HttpContext;
        }
        public async Task<AuthResponseModel> AuthenticateUserAsync(LoginViewModel model)
        {
            Console.WriteLine(_client.BaseAddress);
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

            _client.Dispose();

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
                    new Claim("FullName", $"{authResponse?.User.FirstName} {authResponse?.User.LastName}")
                };



            var currentTokenClaim = _httpContext.User.Claims.FirstOrDefault(c => c.Type == "Token");
            if (currentTokenClaim != null)
            {
                claims.Add(new Claim("Token", authResponse.Token));
            }


            var identity = new ClaimsIdentity(claims, "login");
            var principal = new ClaimsPrincipal(identity);
            await _httpContext.SignInAsync("Cookies", principal);

            _httpContext.Response.Cookies.Append("AccessToken", authResponse.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                
            });

        }

        public async Task<string> RefreshAccessTokenAsync()
        {
            if (!_httpContext.Request.Cookies.TryGetValue("AccessToken", out var token))
            {
                throw new UnauthorizedAccessException("Access token not found.");
            }

            var response = await _client.GetAsync($"RefreshToken?Token={token}");

            if (!response.IsSuccessStatusCode)
            {
                throw new UnauthorizedAccessException("Failed to refresh access token.");
            }

            var content = await response.Content.ReadAsStringAsync();

            var refreshTokenObject = JsonConvert.DeserializeObject<RefreshTokenResponse>(content);

            var newToken = refreshTokenObject.Token;

            if (string.IsNullOrEmpty(newToken))
            {
                throw new UnauthorizedAccessException("Empty token received.");
            }

            //Access token
            _httpContext.Response.Cookies.Append("AccessToken", newToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            //Claims
            var identity = _httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var oldTokenClaim = identity.FindFirst("Token");
                if (oldTokenClaim != null)
                {
                    identity.RemoveClaim(oldTokenClaim);
                }
                identity.AddClaim(new Claim("Token", newToken));

                var principal = new ClaimsPrincipal(identity);
                await _httpContext.SignInAsync("Cookies", principal);
            }

            return newToken;
        }
        
        public async Task<HttpResponseMessage> SendWithRefreshAsync(Func<Task<HttpResponseMessage>> requestFunc)
        {
            var response = await requestFunc(); // Make Request

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshAccessTokenAsync(); // RefreshToken
                response = await requestFunc(); // Reload
            }

            return response;
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
            _client.Dispose();
            return responseContent;
        }

    }
}
