using MessagingProject.Abstractions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using MessagingProject.Models.Email.Template;
using MessagingProject.Models;
using static DevExpress.Data.Helpers.FindSearchRichParser;
using System.Text;
using MessagingProject.Models.Contacts;

namespace MessagingProject.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly HttpClient _client;
        //Cache to store templates
        private static readonly ConcurrentDictionary<int, TemplateDataModel> _templatesCache = new ConcurrentDictionary<int, TemplateDataModel>();

        public TemplateService(HttpClient client)
        {
            _client = client;
        }


        //GET
        public async Task<TemplateResponseModel> GetTemplates(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"GetTemplatesByToken?Token={token}");

            try
            {
                var response = await _client.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<TemplateResponseModel>(content);

                if (responseModel == null)
                {
                    throw new NullReferenceException("Deserialized response is null.");
                }

                // Refresh the cache with the latest templates
                foreach (var template in responseModel.Templates)
                {
                    _templatesCache[template.Id] = template;
                }

                return responseModel;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Error fetching templates.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);
            }
        }

        public async Task<TemplateDataModel> GetTemplatesById(int? id)
        {
           
            if (!_templatesCache.TryGetValue(id.Value, out var template))
            {
                Console.WriteLine(($"Template with id {id} not found."));

                return null;
            }
            

            return await Task.FromResult(template);
        }


        //DELETE
        public async Task<BaseResponseModel> DeleteTemplate(int id,string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"DeleteTemplate?Token={token}&ID={id}");
            try
            {
                var response = await _client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<BaseResponseModel>(content);
                if (responseModel == null)
                {
                    throw new NullReferenceException("Deserialized response is null.");
                }
                // Remove the template from the cache
                _templatesCache.TryRemove(id, out _);
                return responseModel;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Error deleting template.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);
            }
        }

        //CREATE
        //UPDATE
        public async Task<BaseResponseModel> UpdateTemplate(TemplateRequestModel request)
        {
            try
            {
                var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                var json = JsonConvert.SerializeObject(request);


                Console.WriteLine(json);

                var response = await _client.PostAsync("UpdateTemplate", requestContent);

                var contentBody = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(contentBody))
                {
                    Console.WriteLine("Empty response body");
                    return null;
                }

                var responseBody = JsonConvert.DeserializeObject<BaseResponseModel>(contentBody);

                if (responseBody.ErrorCode == 0)
                {
                    return responseBody;
                }

                return null;

            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Error updating template.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);
            }
        }
    }
}
