using Newtonsoft.Json;
using System;

namespace MessagingProject.Models
{
    public class UserResponse
    {
        [JsonProperty("Company")]
        public string Company { get; set; }

        [JsonProperty("CreateDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("LastAuthorize")]
        public DateTime LastAuthorize { get; set; }

        [JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("UiLanguage")]
        public int UiLanguage { get; set; }

    }
}  


