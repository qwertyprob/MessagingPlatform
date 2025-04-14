using MessagingProject.Models.Email;
using Newtonsoft.Json;

namespace MessagingProject.Services
{
    public class TemplateService
    {
        public HttpClient _client { get; set; }
        public TemplateService(HttpClient client)
        {
            _client = client;
        }
        //GET
        public async Task<TemplateResponseModel> GetTemplates(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"MailService/GetTemplatesByToken?Token={token}");
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<TemplateResponseModel>(content);
            if (responseModel is null)
            {
                throw new NullReferenceException(responseModel?.ErrorMessage);
            }
            return responseModel;
        }
    }
}
