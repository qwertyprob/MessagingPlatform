using MessagingProject.Models.Contacts;

namespace MessagingProject.Abstractions
{
    public interface IContactService
    {
        Task<ContactResponseModel> GetContactLists(string token);
        Task<IEnumerable<SingleContactModel>> GetContactList(string token, int id);
    }
}