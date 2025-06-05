using Newtonsoft.Json;

namespace MessagingProject.Models.SMS
{
    public class ESMSettingsResponseModel :BaseResponseModel
    {
        [JsonProperty("esmAlias")]
        public string EsmAlias { get; set; }

        [JsonIgnore]
        public List<string> EsmAliasList =>
        EsmAlias?.Split(',').Select(s => s.Trim()).ToList();

        [JsonProperty("externalSecurityPolicy")]
        public string ExternalSecurityPolicy { get; set; }

        [JsonProperty("companyShortName")]
        public string CompanyShortName { get; set; }
    } 
}
