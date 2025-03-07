using Newtonsoft.Json;

namespace MessagingProject.Models
{
    public class UserClaimsInfoResponse
    {
        public string Email { get; set; }
        public string Company { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string UiLanguage { get; set; }
        public string Token { get; set; }
    }
    
}
