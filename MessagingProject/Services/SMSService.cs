using MessagingProject.Models;
using MessagingProject.Models.SMS;
using Newtonsoft.Json;

namespace MessagingProject.Services
{
    public class SMSService : ISMSService
    {
        private readonly HttpClient _client;
        public SMSService(HttpClient client)
        {
            _client = client;
        }
        //GET
        public async Task<GetByMonthInfoResponseModel> GetByMonthInfo(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"InfoGetByCompany?Token={token}");
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
            var request = new HttpRequestMessage(HttpMethod.Get, $"InfoGetByCompany?Token={token}");
            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<GetByWeekInfoResponseModel>(content);

            if (responseModel is null)
            {

                throw new NullReferenceException(responseModel?.ErrorMessage);
            }

            return responseModel;

        }
        //GET GAMPAIGN 
        public async Task<CampaignSmsResponseModel> GetSmsGampaign(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"Campaigns?Token={token}");

            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<CampaignSmsResponseModel>(content);
            if (responseModel is null)
            {
                throw new NullReferenceException(responseModel?.ErrorMessage);
            }
            if (responseModel.ErrorCode != 0)
            {
                throw new Exception(responseModel?.ErrorMessage);
            }
            
            return responseModel;
        }
    }
}
