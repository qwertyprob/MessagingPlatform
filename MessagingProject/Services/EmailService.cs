using MessagingProject.Abstractions;
using MessagingProject.Models;
using MessagingProject.Models.Email;
using Newtonsoft.Json;
using static DevExpress.Data.Helpers.FindSearchRichParser;

namespace MessagingProject.Services
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _client;
        private readonly ITemplateService _templateService;
        public EmailService(HttpClient client, ITemplateService templateService)
        {
            _client = client;
            _templateService = templateService;
        }
        //GET DATA
        public async Task<GetByMonthInfoResponseModel> GetByMonthInfo(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"GetInfoCompany?Token={token}&CampaignOnly=true");

            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<GetByMonthInfoResponseModel>(content);

            if (responseModel is null)
            {

                throw new NullReferenceException(responseModel?.ErrorMessage);
            }

            return responseModel;

        }

        public async Task<GetByWeekInfoResponseModel> GetByWeekInfo(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"GetInfoCompany?Token={token}&CampaignOnly=true");
            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<GetByWeekInfoResponseModel>(content);

            if (responseModel is null)
            {

                throw new NullReferenceException(responseModel?.ErrorMessage);
            }

            return responseModel;

        }

        //GET CAMPAIGNS 
        public async Task<CampaignResponseModel> GetCampaigns(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"Campaign/GetList?Token={token}");
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CampaignResponseModel>(content);
            if (responseModel is null)
            {
                throw new NullReferenceException();
            }

            var templateList = await _templateService.GetTemplates(token);
            foreach (var model in responseModel.CampaignDataList)
            {
                var template = await _templateService.GetTemplatesById(model.Template);
                if (template == null)
                {
                    continue;
                }
                

                model.TemplateName = template.Name;

                
                
            }

            return responseModel;
        }
    }
}
