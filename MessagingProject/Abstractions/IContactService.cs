using MessagingProject.Models;
using MessagingProject.Models.Contacts;

namespace MessagingProject.Abstractions
{
    public interface IContactService
    {
        Task<ContactResponseModel> GetContactLists(string token);
        Task<IEnumerable<SingleContactModel>> GetContactList(string token, int id);
        Task<BaseResponseModel> DeleteContactList(string token, int id);
        Task<SingleContactResponseModel> GetDeleteContactList(string token, int id);

    }
}