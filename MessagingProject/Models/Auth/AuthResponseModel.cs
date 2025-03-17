using Newtonsoft.Json;

namespace MessagingProject.Models.Auth
{
    public class AuthResponseModel:BaseResponseModel
    {
        [JsonProperty("Token")]
        public required string Token { get; set; }
        
        [JsonProperty("User")]
        public required UserResponse User { get; set; }
    }
}
