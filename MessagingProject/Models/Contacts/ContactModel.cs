using MessagingProject.Models.Email;
using MessagingProject.Models.SMS;
using Newtonsoft.Json;

namespace MessagingProject.Models.Contacts
{
    public class ContactModel
    {

        [JsonProperty("id")]
        public required int Id { get; set; }
        [JsonProperty("name")]
        public required string Name { get; set; }
        [JsonProperty("email")]
        public required int Emails { get; set; }
        [JsonProperty("description")]
        public required string Description { get; set; }
        [JsonProperty("phone")]
        public required int Phones { get; set; }
        [JsonProperty("createDate")]
        public required DateTime CreateDate { get; set; }

        public string ParsedDate => CreateDate.ToString("dd-MM-yyyy HH:mm");



    }
}
