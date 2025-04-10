using Newtonsoft.Json;

namespace MessagingProject.Models.Email
{
    public class CampaignResponseModel
    {
        [JsonProperty("campaignDataList")]
        public IEnumerable<CampaignData> CampaignDataList { get; set; } = [];
    }

    public class CampaignData
    {
        //[JsonProperty("contactListData")]
        //public IEnumerable<string> ContactListData { get; set; }

        //[JsonProperty("token")]
        //public string Token { get; set; } - always null

        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("subject")]
        public string Subject { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("status")]
        private int Status { get; set; }
        public string StatusDescription => Status switch
        {
            1 => "Запланнированный",
            2 => "Мгновенный",
            _ => "Черновик"
        };

        [JsonProperty("created")]
        private DateTime? Created { get; set; }
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
