using MessagingProject.Abstractions;
using MessagingProject.Models;
using MessagingProject.Models.Email;
using Newtonsoft.Json;
using System.Net;
using System.Text;
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

            // Instead of using a foreach loop, we can use LINQ to create a list of tasks
            var tasks = responseModel.CampaignDataList.Select(async model =>
            {
                var template = await _templateService.GetTemplatesById(model.Template);
                if (template != null)
                {
                    model.TemplateName = template.Name;
                }
            });

            await Task.WhenAll(tasks);

            return responseModel;
        }
        public async Task<CampaignData?> GetCampaignById(string token, string id)
        {
            var campaigns = await this.GetCampaigns(token);

            return campaigns.CampaignDataList.FirstOrDefault(c => c.Id == id);
        }

        //DELETE CAMPAIGNS 
        public async Task<BaseResponseModel> DeleteCampaignById(string token, string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"Campaign/Delete?Token={token}&ID={id}");

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Bad Response");
                
            }
            var content = await response.Content.ReadAsStringAsync();

            var message = JsonConvert.DeserializeObject<BaseResponseModel>(content);

            if(message.ErrorCode != 0)
            {
                Console.WriteLine(message.ErrorMessage);
            }


            return message;

        }

        public async Task<BaseResponseModel> UpsertCampaign(CampaignRequestModel request)
        {
            try
            {
                var json = JsonConvert.SerializeObject(request);
                var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("Campaign/Upsert", requestContent);

                if (!response.IsSuccessStatusCode&& response.StatusCode != HttpStatusCode.Unauthorized)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Bad request: Status - {response.StatusCode}, Response - {errorContent}");
                }

                var content = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(content))
                {
                    throw new Exception("Bad response: content is null or empty.");
                }

                var responseModel = JsonConvert.DeserializeObject<BaseResponseModel>(content);
                return responseModel ?? throw new Exception("Deserialized response is null.");
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("HTTP request error while updating campaign.", ex);
            }
            catch (JsonException ex)
            {
                throw new ApplicationException("JSON deserialization error.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during campaign upsert.", ex);
            }
        }


    }
}
