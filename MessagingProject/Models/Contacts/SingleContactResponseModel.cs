using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MessagingProject.Models.Contacts
{
    public class SingleContactResponseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("contactsList")]
        public required ContactData ContactData { get; set; }
    }
    public class ContactData
    {
        [JsonProperty("contactsData")]
        public required string HashedContactData { get; set; }
    }

}
