using Newtonsoft.Json;

namespace MessagingProject.Models
{
    public class AuthResponseModel
    {
        [JsonProperty("Token")]
        public required string Token { get; set; }
        [JsonProperty("ErrorMessage")]
        public required string ErrorMessage { get; set; }
        [JsonProperty("User")]
        public required UserResponse User { get; set; }
    }
}
