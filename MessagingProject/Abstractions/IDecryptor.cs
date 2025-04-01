using MessagingProject.Models.Contacts;

namespace MessagingProject.Abstractions
{
    public interface IDecryptor
    {
        string DecodeHashedData(string hashedData);
        string EncodeToBase64(SingleContactModel contact);
        IEnumerable<SingleContactModel> DecodeHashedDataToList(string hashedData);
        string EncodeListToBase64(IEnumerable<SingleContactModel> contactList);
    }
}