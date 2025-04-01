using Newtonsoft.Json;

namespace MessagingProject.Models.Contacts
{
    public class CreateSingleContactRequest
    {
        [JsonProperty("request")]
        public SingleContactModel Request { get; set; }

        [JsonProperty("requestBody")]
        public ContactsList RequestBody { get; set; }
    }
}
