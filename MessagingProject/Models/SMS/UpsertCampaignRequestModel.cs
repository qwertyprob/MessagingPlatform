using Newtonsoft.Json;

namespace MessagingProject.Models.SMS
{
    public class UpsertCampaignRequestModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shortName")]
        public string ShortName { get; set; }

        [JsonProperty("scheduledDate")]
        public DateTime ScheduledDate { get; set; }

        [JsonProperty("reviewerComments")]
        public string ReviewerComments { get; set; } = null;

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("notificationType")]
        public int NotificationType { get; set; } = 0;

        [JsonProperty("phoneList")]
        public string PhoneList { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; } = 0;

        [JsonProperty("contactListID")]
        public string ContactListID { get; set; }

        [JsonIgnore]
        public string ContactListNames { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
