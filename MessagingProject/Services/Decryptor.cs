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
        public IEnumerable<SingleContactModel> DecodeHashedDataToList(string hashedData)
        {
            try
            {
                var decodedString = Encoding.UTF8.GetString(Convert.FromBase64String(hashedData));
                var result = JsonConvert.DeserializeObject<IEnumerable<SingleContactModel>>(decodedString);
                return result ?? Enumerable.Empty<SingleContactModel>();
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

        public string EncodeListToBase64(IEnumerable<SingleContactModel> contactList)
        {
            try
            {
                string json = JsonConvert.SerializeObject(contactList);

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
