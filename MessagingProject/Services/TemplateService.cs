using MessagingProject.Abstractions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using MessagingProject.Models.Email.Template;
using MessagingProject.Models;

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

        public async Task<TemplateDataModel> GetTemplatesById(int id)
        {
            if (!_templatesCache.TryGetValue(id, out var template))
            {
                throw new KeyNotFoundException($"Template with id {id} not found.");
            }

            return template;
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
    }
}
