using Newtonsoft.Json;

namespace MessagingProject.Models.Contacts
{
    public class SingleContactModel
    {
        [JsonProperty("Id")]
        public required int Id { get; set; }
        [JsonProperty("Name")]
        public required string Name { get; set; }
        [JsonProperty("Email")]
        public required string Email { get; set; }
        [JsonProperty("Phone")]
        public required string Phone { get; set; }
    }
}
