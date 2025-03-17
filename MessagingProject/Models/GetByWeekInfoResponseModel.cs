using Newtonsoft.Json;

namespace MessagingProject.Models
{
    public class GetByWeekInfoResponseModel :BaseResponseModel
    {
        [JsonProperty("sentToday")]
        public required int SentToday { get; set; }
        [JsonProperty("sentThisWeek")]
        public required int SentThisWeek { get; set; }
    }
}
