using Newtonsoft.Json;

namespace MessagingProject.Models.Contacts
{
    public class ContactResponseModel : BaseResponseModel
    {
        

        [JsonProperty("contactsLists")]
        public List<ContactModel> ContactsLists { get; set; }

    }
}
