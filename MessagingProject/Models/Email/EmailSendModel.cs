using MessagingProject.Models.Email.Template;
using Newtonsoft.Json;

namespace MessagingProject.Models.Email
{
    public class EmailSendModel
    {
        //Templates 
        public IEnumerable<TemplateDataModel> Templates { get; set; } = [];

        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("template")]
        public int Template { get; set; }
        [JsonProperty("subject")]
        public string Subject { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("created")]
        public DateTime? Created { get; set; }
        public string FormattedCreateParsedDate => Created?.ToString("dd-MM-yyyy HH:mm");

        [JsonProperty("scheduled")]
        private DateTime? Scheduled { get; set; }

        public string FormattedScheduledParsedDate => Scheduled?.ToString("dd-MM-yyyy HH:mm");


        [JsonProperty("emails")]
        public int Emails { get; set; }

        [JsonProperty("contactListID")]
        public string ContactListID { get; set; }
        [JsonIgnore]
        public string TemplateName { get; set; } = "";

        [JsonProperty("contactList")]
        public string ContactListData { get; set; }

    }
}