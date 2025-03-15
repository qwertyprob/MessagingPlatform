using Newtonsoft.Json;

namespace MessagingProject.Models.Contacts
{
    public class ContactResponseModel
    {
        [JsonProperty("errorMessage")]
        public required string ErrorMessage { get; set; }

        [JsonProperty("errorCode")]
        public required int ErrorCode { get; set; }

        [JsonProperty("contactsLists")]
        public List<ContactModel> ContactsLists { get; set; }

    }
}
