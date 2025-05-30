using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;

namespace MessagingProject.Models.SMS
{
    public class CampaignSmsResponseModel : BaseResponseModel
    {
        [JsonProperty("campaignList")]
        public List<CampaignModel> CampaignList { get; set; }
    }

    public class CampaignModel 
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("reviewedByUser")]
        public string ReviewedByUser { get; set; }

        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }

        public string FormattedCreateParsedDate => CreateDate.ToString("dd-MM-yyyy HH:mm");


        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shortName")]
        public string ShortName { get; set; }

        [JsonProperty("scheduledDate")]
        public DateTime ScheduledDate { get; set; }

        public string FormattedScheduledParsedDate => ScheduledDate.ToString("dd-MM-yyyy HH:mm");


        [JsonProperty("reviewerComments")]
        public string ReviewerComments { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("notificationType")]
        public int NotificationType { get; set; }

        [JsonProperty("phoneList")]
        public string PhoneList { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("statusDescription")]
        public string StatusDescription => Status switch
        {
            1 => MessagingProject.Resources.Resource.schelduled,
            2 => MessagingProject.Resources.Resource.instant,
            _ => MessagingProject.Resources.Resource.draft
        };


        [JsonProperty("contactListID")]
        public string ContactListID { get; set; }
    }

}