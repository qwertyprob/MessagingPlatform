using Newtonsoft.Json;
using System;

namespace MessagingProject.Models.Auth
{
    public class  UserResponse
    {
        [JsonProperty("Company")]
        public required string Company { get; set; }

        [JsonProperty("CreateDate")]
        public required DateTime CreateDate { get; set; }

        [JsonProperty("ID")]
        public required int ID { get; set; }

        [JsonProperty("Email")]
        public required string Email { get; set; }

        [JsonProperty("FirstName")]
        public required string FirstName { get; set; }

        [JsonProperty("LastName")]
        public required string LastName { get; set; }

        [JsonProperty("LastAuthorize")]
        public required DateTime LastAuthorize { get; set; }

        [JsonProperty("PhoneNumber")]
        public required string PhoneNumber { get; set; }

        [JsonProperty("UiLanguage")]
        public required int UiLanguage { get; set; }

    }
}  


