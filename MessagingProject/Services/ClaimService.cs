using System.Security.Claims;
using MessagingProject.Models;

namespace MessagingProject.Services
{
    public class ClaimsService
    {
        public List<Claim> GenerateClaims(AuthResponseModel authResponse, LoginViewModel model)
        {
            return new List<Claim>
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
        }
    }
}
