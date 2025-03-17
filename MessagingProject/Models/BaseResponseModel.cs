using Newtonsoft.Json;

namespace MessagingProject.Models
{
    public class BaseResponseModel
    {
        [JsonProperty("ErrorMessage")]
        public required string ErrorMessage { get; set; }
        [JsonProperty("ErrorCode")]
        public required int ErrorCode { get; set; }
    }
}