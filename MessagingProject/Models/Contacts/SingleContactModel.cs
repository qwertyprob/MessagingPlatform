using Newtonsoft.Json;

namespace MessagingProject.Models.Contacts
{
    public class SingleContactModel
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Name")]
        public  string Name { get; set; }
        [JsonProperty("Email")]
        public  string Email { get; set; }
        [JsonProperty("Phone")]
        public  string Phone { get; set; }
    }
}
