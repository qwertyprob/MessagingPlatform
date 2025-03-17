using Newtonsoft.Json;

namespace MessagingProject.Models
{
    //Email & SMS
    public class GetByMonthInfoResponseModel : BaseResponseModel
    {
        
        [JsonProperty("rejected")]
        public required int Rejected { get; set; }
        [JsonProperty("failedDelivery")]
        public required int FailedDelivery { get; set; }
        [JsonProperty("waitingForSend")]
        public required int WaitingForSend { get; set; }
        [JsonProperty("readSentThisMonth")]
        public required int ReadSentThisMonth { get; set; }
        [JsonProperty("incomeThisMonth")]
        public required int IncomeThisMonth { get; set; }
        [JsonProperty("sentThisMonth")]
        public required int SentThisMonth { get; set; }
    }
}
