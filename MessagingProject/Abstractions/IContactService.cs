using MessagingProject.Models;
using MessagingProject.Models.Contacts;
using MessagingProject.Services;
using Newtonsoft.Json;
using System.Text;

namespace MessagingProject.Abstractions
{
    public interface IContactService
    {
        Task<BaseResponseModel> DeleteSingleContactList(DeleteSingleContactRequest request);
        
        Task<BaseResponseModel> DeleteContactList(string token, int id);
        Task<ContactResponseModel> GetContactLists(string token);

        Task<IEnumerable<SingleContactModel>> GetContactHashedData(string token, int id);
        Task<string> GetEmailsFromContactListAsync(string token, string contactList);

        Task<SingleContactResponseModel> GetContactList(string token, int id);

        Task<ContactResponseModel> CreateContactList(ContactsList request);
        Task<BaseResponseModel> CreateSingleContactList(CreateSingleContactRequest request);

    }
}