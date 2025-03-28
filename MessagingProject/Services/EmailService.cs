﻿using MessagingProject.Abstractions;
using MessagingProject.Models;
using Newtonsoft.Json;
using static DevExpress.Data.Helpers.FindSearchRichParser;

namespace MessagingProject.Services
{
    public class EmailService : IEmailService
    {
        public HttpClient _client { get; set; }

        public EmailService(HttpClient client)
        {
            _client = client;
        }

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
    }
}
