using Newtonsoft.Json;

namespace MessagingProject.Models.Contacts
{
    public class DeleteSingleContactRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("contactId")]
        public int ContactId { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
