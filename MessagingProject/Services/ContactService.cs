using MessagingProject.Abstractions;
using MessagingProject.Models.Contacts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
namespace MessagingProject.Services
{
    public class ContactService : IContactService
    {
        private readonly HttpClient _client;
        private readonly IUserService _userService;
        private readonly IDecryptor _decryptor;

        public ContactService(HttpClient client, IUserService userService, IDecryptor decryptor)
        {
            _client = client;
            _decryptor = decryptor;
            _userService = userService;
            

        }

        public async Task<ContactResponseModel> GetContactLists(string token)
        {
            
            var request = new HttpRequestMessage(HttpMethod.Get, $"GetContactLists?Token={token}");
            
            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            var contactResponse = JsonConvert.DeserializeObject<ContactResponseModel>(content);

            if (contactResponse is null)
            {
                throw new NullReferenceException("Contact list is null!");
            }
           

            if (string.IsNullOrWhiteSpace(content) || content.StartsWith("<"))
            {
                throw new Exception("Unexpected response format (probably HTML).");
            }


            _client.Dispose();

            return contactResponse;

        }

        public async Task<IEnumerable<SingleContactModel>> GetContactList(string token, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"GetContactList?Token={token}&ID={id}");

            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<SingleContactResponseModel>(content);

            if (responseModel is not null && !string.IsNullOrEmpty(responseModel.ContactData.HashedContactData))
            {
                var decodedUsersString = _decryptor.DecodeHashedData(responseModel.ContactData.HashedContactData);
               
                var listOfContacts = JsonConvert.DeserializeObject<IEnumerable<SingleContactModel>>(decodedUsersString);
               
                return listOfContacts;
            }
            

            return Enumerable.Empty<SingleContactModel>();
        }

        



    }
}
