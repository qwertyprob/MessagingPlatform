using MessagingProject.Abstractions;
using MessagingProject.Models.Email;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MessagingProject.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly HttpClient _client;
        private static readonly ConcurrentDictionary<int, TemplateDataModel> _templatesCache = new ConcurrentDictionary<int, TemplateDataModel>();

        public TemplateService(HttpClient client)
        {
            _client = client;
        }

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
    }
}
