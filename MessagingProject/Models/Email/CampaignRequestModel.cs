using Newtonsoft.Json;

namespace MessagingProject.Models.Email
{
    public class CampaignRequestModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

            [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("scheduled")]
        public DateTimeOffset Scheduled { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("emails")]
        public int Emails { get; set; }

        [JsonProperty("template")]
        public int Template { get; set; }

        [JsonProperty("contactListID")]
        public string ContactListID { get; set; }
        [JsonProperty("contactList")]
        public string ContactList { get; set; }

        [JsonProperty("replyTo")]
        public string ReplyTo { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
