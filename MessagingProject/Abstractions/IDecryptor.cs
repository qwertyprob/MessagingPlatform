using MessagingProject.Models.Contacts;

namespace MessagingProject.Abstractions
{
    public interface IDecryptor
    {
        string DecodeHashedData(string hashedData);
        string EncodeToBase64(SingleContactModel contact);
    }
}