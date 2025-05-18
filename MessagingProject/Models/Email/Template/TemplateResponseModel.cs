using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;

namespace MessagingProject.Models.Email.Template
{
    public class TemplateResponseModel : BaseResponseModel
    {
        [JsonProperty("templates")]
        public IEnumerable<TemplateDataModel> Templates { get; set; } = [];
    }

    public class TemplateDataModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name => string.IsNullOrEmpty(Name) ? "[Удалённый шаблон]" : Name;

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("bodyJson")]
        public string BodyJson { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("state")]
        public int State { get; set; }

        [JsonProperty("imageTemplate")]
        public string ImageTemplate { get; set; }


    }
}
