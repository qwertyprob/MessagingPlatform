using MessagingProject.Abstractions;
using System.Text;

namespace MessagingProject.Services
{
    public class Decryptor : IDecryptor
    {
        public string DecodeHashedData(string hashedData)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(hashedData));
            }
            catch (FormatException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid Base64 string: {hashedData}");
                Console.ForegroundColor = ConsoleColor.White;
                throw new Exception("Invalid Base64 encoding", ex);
            }

        }


    }
}
