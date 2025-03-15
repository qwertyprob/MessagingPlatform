namespace MessagingProject.Abstractions
{
    public interface IDecryptor
    {
        string DecodeHashedData(string hashedData);
    }
}