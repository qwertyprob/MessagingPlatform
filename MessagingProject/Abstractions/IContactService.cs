using MessagingProject.Models;
using MessagingProject.Models.Contacts;

namespace MessagingProject.Abstractions
{
    public interface IContactService
    {
        Task<BaseResponseModel> DeleteContactList(string token, int id);
        Task<ContactResponseModel> GetContactLists(string token);

        Task<IEnumerable<SingleContactModel>> GetContactHashedData(string token, int id);

        Task<SingleContactResponseModel> GetDeleteContactList(string token, int id);

        Task<BaseResponseModel> CreateContactList(CreateContactListRequest request);

    }
}