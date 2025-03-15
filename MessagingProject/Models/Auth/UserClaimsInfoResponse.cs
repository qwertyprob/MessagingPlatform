using Newtonsoft.Json;

namespace MessagingProject.Models.Auth
{
    public class UserClaimsInfoResponse
    {
        public required string Email { get; set; }
        public required string Company { get; set; }
        public required string FullName { get; set; }
        public required string Password { get; set; }
        public required string UiLanguage { get; set; }
        public required string Token { get; set; }
    }
    
}
