using DevExtreme.AspNet.Mvc;
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

            if (!response.IsSuccessStatusCode)
            {
                
                throw new HttpRequestException($"Bad request:{response.StatusCode}");
            }

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

        public async Task<CampaignModel?> GetSmsGampaignById(string token, int id)
        {
            var campaignsResponse = await GetSmsGampaign(token);

            var campaign = campaignsResponse.CampaignList.FirstOrDefault(x => x.Id == id);

            if (campaign == null)
            {
                return null; 
            }

            return campaign;
        }


        //GET ALIAS
        public async Task<ESMSettingsResponseModel> GetESMSettings(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"CampaignGetESMSettings?Token={token}");

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {

                throw new HttpRequestException($"Bad request:{response.StatusCode}");
            }
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(content))
            {
                throw new JsonSerializationException($"Serialize exception {content}");
            }

            var responseModel = JsonConvert.DeserializeObject<ESMSettingsResponseModel>(content);
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

        //DELETE CAMPAIGN 
        public async Task<BaseResponseModel> DeleteSmsCampaign(string token, int id)
        {
            var response = await _client.DeleteAsync($"Campaign?Token={token}&ID={id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Bad request:{response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<BaseResponseModel>(content);
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
