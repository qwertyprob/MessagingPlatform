using MessagingProject.Abstractions;
using MessagingProject.Models;
using MessagingProject.Models.Contacts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
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
        //DELETE
        
        public async Task<BaseResponseModel> DeleteContactList(string token, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"DeleteContactList?Token={token}&ID={id}");
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var contactResponse = JsonConvert.DeserializeObject<BaseResponseModel>(content);

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

        //GET
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

        public async Task<IEnumerable<SingleContactModel>> GetContactHashedData(string token, int id)
        {
            try
            {

                var request = new HttpRequestMessage(HttpMethod.Get, $"GetContactList?Token={token}&ID={id}");

                var response = await _client.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                var responseModel = JsonConvert.DeserializeObject<SingleContactResponseModel>(content);

                if (responseModel is not null && !string.IsNullOrEmpty(responseModel.ContactsList.HashedContactData))
                {
                    var decodedUsersString = _decryptor.DecodeHashedData(responseModel.ContactsList.HashedContactData);

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
        public async Task<SingleContactResponseModel> GetContactList(string token, int id)
        {
            try
            {

                var request = new HttpRequestMessage(HttpMethod.Get, $"GetContactList?Token={token}&ID={id}");

                var response = await _client.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                var responseModel = JsonConvert.DeserializeObject<SingleContactResponseModel>(content);

                if(responseModel != null)
                {
                    return responseModel;
                }
            }

            catch (Exception ex)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }


            return new SingleContactResponseModel();

        }


        //CREATE
        public async Task<ContactResponseModel> CreateContactList(ContactsList request)
        {
            var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("UpdateContactList", requestContent);

            var contentBody = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(contentBody))
            {
                Console.WriteLine("Empty response body");
                return null;
            }

            var responseBody = JsonConvert.DeserializeObject<ContactResponseModel>(contentBody);

            if (responseBody.ErrorCode == 0)
            {
                return responseBody;
            }

            return null;
        }

        public async Task<BaseResponseModel> CreateSingleContactList(CreateSingleContactRequest request)
        {

            request.RequestBody.HashedContactData = _decryptor.EncodeToBase64(request.Request);

            var requestContent = new StringContent(JsonConvert.SerializeObject(request.RequestBody), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("UpdateContactList", requestContent);
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

    }






}

