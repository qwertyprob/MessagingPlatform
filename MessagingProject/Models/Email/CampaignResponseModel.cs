using Newtonsoft.Json;

namespace MessagingProject.Models.Email
{
    public class CampaignResponseModel:BaseResponseModel
    {
        [JsonProperty("campaignDataList")]
        public IEnumerable<CampaignData> CampaignDataList { get; set; } = [];
    }

    public class CampaignData
    {
        [JsonIgnore]
        public string TemplateName { get; set; } = "";

        [JsonProperty("contactList")]
        public string ContactListData { get; set; } 

        //[JsonProperty("token")]
        //public string Token { get; set; } - always null

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
        [JsonProperty("status")]
        private int Status { get; set; }
        [JsonProperty("statusDescription")]
        public string StatusDescription => Status switch
        {
            1 => MessagingProject.Resources.Resource.schelduled,
            2 => MessagingProject.Resources.Resource.instant,
            _ => MessagingProject.Resources.Resource.draft
        };

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


    

    }

    
}
