using MessagingProject.Abstractions;
using MessagingProject.Models;
using MessagingProject.Models.Contacts;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<BaseResponseModel> DeleteSingleContactList(DeleteSingleContactRequest request)
        {
            var contactToDelete = GetContactList(request.Token, request.Id).Result.ContactsList;

            //new list
            var listOfHashedData = RemoveContact(_decryptor.DecodeHashedDataToList(contactToDelete.ContactsData), request.ContactId);

            var newContactsData = _decryptor.EncodeListToBase64(listOfHashedData);

            contactToDelete.ContactsData = newContactsData;

            contactToDelete.Phone = PhoneCount(listOfHashedData);
            contactToDelete.Email = EmailCount(listOfHashedData);
            contactToDelete.Token = request.Token;

            var requestContent = new StringContent(JsonConvert.SerializeObject(contactToDelete), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("UpdateContactList", requestContent);

            var contentBody = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(contentBody))
            {
                Console.WriteLine("Empty response body");
                return null;
            }

            var responseBody = JsonConvert.DeserializeObject<BaseResponseModel>(contentBody);



            return responseBody;




        }

        private IEnumerable<SingleContactModel> RemoveContact(IEnumerable<SingleContactModel> list, int id)
        {
            var listToReturn = list.ToList();

            if (id <= 0 || id > listToReturn.Count)
            {
                Console.WriteLine("Invalid contact ID");
                return listToReturn; 
            }

            listToReturn.RemoveAt(id - 1);

            SetIdToList(listToReturn);

            return listToReturn;
        }



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

                if (responseModel is not null && !string.IsNullOrEmpty(responseModel.ContactsList.ContactsData))
                {
                    var decodedUsersString = _decryptor.DecodeHashedData(responseModel.ContactsList.ContactsData);

                    var listOfContacts = JsonConvert.DeserializeObject<IEnumerable<SingleContactModel>>(decodedUsersString) ?? null;

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

        //FOR SENDEMAIL
        public async Task<string> GetEmailsFromContactListAsync(string token,string contactList)
        {
            if (string.IsNullOrWhiteSpace(contactList))
                return string.Empty;

            var ids = contactList
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(idStr => int.TryParse(idStr, out var id) ? id : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList();

            var allEmails = new List<string>();

            foreach (var id in ids)
            {
                var contacts = await GetContactHashedData(token,id);

                if (contacts != null)
                {
                    var emails = contacts
                        .Where(c => !string.IsNullOrWhiteSpace(c.Email))
                        .Select(c => c.Email);

                    allEmails.AddRange(emails);
                }
                else
                {
                    return string.Empty;
                }
            }

            return string.Join(",", allEmails.Distinct()); 
        }




        //CREATE AND UPDATE(NAME)
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

            var listOfHashedData = AddOrUpdateData(request.Request, request.RequestBody.ContactsData);



            request.RequestBody.ContactsData = _decryptor.EncodeListToBase64(listOfHashedData);

            request.RequestBody.Phone = PhoneCount(listOfHashedData);
            request.RequestBody.Email = EmailCount(listOfHashedData);

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

        //PRIVATE METHODS
        private IEnumerable<SingleContactModel> AddOrUpdateData(SingleContactModel user, string base64)
        {
            if (!string.IsNullOrEmpty(base64))
            {
                var list = _decryptor.DecodeHashedDataToList(base64).ToList();

                var existingUser = list.FirstOrDefault(u => u.Id == user.Id);

                if (existingUser != null)
                {
                    existingUser.Name = user.Name;
                    existingUser.Phone = user.Phone;
                    existingUser.Email = user.Email;
                }
                else
                {
                    list.Add(user);
                }

                SetIdToList(list);
                return list;
            }

            return new List<SingleContactModel>();
        }


        private int EmailCount(IEnumerable<SingleContactModel> list)
        {
            return list?.Count(x => x != null && !string.IsNullOrWhiteSpace(x.Email)) ?? 0;
        }

        private int PhoneCount(IEnumerable<SingleContactModel> list)
        {
            return list?.Count(x => x != null && !string.IsNullOrWhiteSpace(x.Phone)) ?? 0;
        }

        private void SetIdToList(List<SingleContactModel> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Id = i + 1; 
            }
        }


    }






}

