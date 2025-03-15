using MessagingProject.Abstractions;
using System.Text;

namespace MessagingProject.Services
{
    public class Decryptor : IDecryptor
    {
        public string DecodeHashedData(string hashedData)
        {
            var data = Encoding.UTF8.GetString(Convert.FromBase64String(hashedData));
            return data;
        }
    }
}
