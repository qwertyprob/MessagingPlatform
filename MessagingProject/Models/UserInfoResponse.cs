using Newtonsoft.Json;

namespace MessagingProject.Models
{
    public class UserClaimsInfoResponse
    {
        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Company")]
        public string Company { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Password { get; set; }

        [JsonProperty("UiLanguage")]
        public int UiLanguage { get; set; }

        [JsonProperty("Token")]
        public string Token { get; set; }
    }
}
