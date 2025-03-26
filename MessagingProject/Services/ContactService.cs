using MessagingProject.Abstractions;
using MessagingProject.Models.Contacts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using static DevExpress.Data.Helpers.FindSearchRichParser;
namespace MessagingProject.Services
{
    public class ContactService : IContactService
    {
        private readonly HttpClient _client;
        private readonly IDecryptor _decryptor;
        private readonly IUserService _userService;

        public ContactService(HttpClient client, IDecryptor decryptor, IUserService userService)
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



            return contactResponse;

        }

        public async Task<IEnumerable<SingleContactModel>> GetContactList(string token, int id)
        {
            try
            {
                Console.WriteLine($"Token: {token}");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = new HttpRequestMessage(HttpMethod.Get, $"GetContactList?Token={token}&ID={id}");

                var response = await _client.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();


                Console.WriteLine("Raw JSON Response: " + content);


                var responseModel = JsonConvert.DeserializeObject<SingleContactResponseModel>(content);

                if (responseModel is not null && !string.IsNullOrEmpty(responseModel.ContactData.HashedContactData))
                {
                    var decodedUsersString = _decryptor.DecodeHashedData(responseModel.ContactData.HashedContactData);
                    Console.WriteLine("Decoded string: " + decodedUsersString);

                    var listOfContacts = JsonConvert.DeserializeObject<IEnumerable<SingleContactModel>>(decodedUsersString);


                    return listOfContacts;
                }
            }

            catch (Exception ex) 
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }

            
            

            return Enumerable.Empty<SingleContactModel>();
        }

        



    }
}
