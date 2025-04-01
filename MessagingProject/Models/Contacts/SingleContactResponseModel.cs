using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MessagingProject.Models.Contacts
{
    public class SingleContactResponseModel
    {
        [JsonProperty("contactsList")]
        public ContactsList ContactsList { get; set; }  
    }

    public class ContactsList
    {

        [JsonProperty("contactsData")]
        public string ContactsData { get; set; } = "W10=";

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("email")]
        public int Email { get; set; }

        [JsonProperty("phone")]
        public int Phone { get; set; }

        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public string Token { get; set; }
    }


}
