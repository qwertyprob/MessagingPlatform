using MessagingProject.Abstractions;
using MessagingProject.Models.Contacts;
using Newtonsoft.Json;
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
        public string EncodeToBase64(SingleContactModel contact)
        {
            try
            {
                string json = JsonConvert.SerializeObject(contact);

                string base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

                return base64Encoded;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error during encoding: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
                throw;
            }
        }


    }
}
