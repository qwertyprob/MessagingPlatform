using Newtonsoft.Json;

namespace MessagingProject.Models
{
    public class AuthResponseModel
    {
        [JsonProperty("Token")]
        public string Token { get; set; }
        [JsonProperty("ErrorMessage")]
        public string ErrorMessage { get; set; }
        [JsonProperty("User")]
        public UserResponse User { get; set; }
    }
}
