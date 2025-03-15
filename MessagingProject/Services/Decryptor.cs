using MessagingProject.Abstractions;
using System.Text;

namespace MessagingProject.Services
{
    public class Decryptor : IDecryptor
    {
        public string DecodeHashedData(string hashedData)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(hashedData));
        }
    }
}
