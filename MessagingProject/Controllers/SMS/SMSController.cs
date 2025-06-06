using MessagingProject.Abstractions;
using MessagingProject.DIContainer;
using MessagingProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace MessagingProject.Controllers.SMS
{
    [Route("SMS")]

    [Authorize]
    public class SMSController : BaseController
    {
        readonly IAuthService _authService;
        readonly IUserService _userService;
        readonly ISMSService _smsService;
        readonly IContactService _contactService;
        public SMSController(IAuthService authService, IUserService userService, ISMSService smsService, IContactService contactService) : base(authService)
        {
            _authService = authService;
            _userService = userService;
            _smsService = smsService;
            _contactService = contactService;
        }
        [Route("CreateSms")]
        public IActionResult CreateSms()
        {
            return View();
        }
        [Route("SmsNewsletters")]
        public IActionResult SmsNewsletters()
        {

            return View();
        }
        //GET
        [HttpGet]
        [Route("GetSmsCampaigns")]
        public async Task<IActionResult> GetSmsCampaigns()
        {
            var token = _userService.GetToken();

            try
            {
                var responseInfo = await _smsService.GetSmsGampaign(token);

                if (responseInfo.ErrorCode == 0)
                {

                    return Ok(responseInfo.CampaignList);
                }

                return BadRequest(responseInfo.ErrorMessage);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Unauthorized: {ex.Message}");
                return Unauthorized();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
        [HttpGet]
        [Route("GetSmsCampaign/{id}")]
        public async Task<IActionResult> GetGampaignById(int id)
        {
            var token = _userService.GetToken();

            try
            {
                var response = await _smsService.GetSmsGampaignById(token, id);

                if(response is not null)
                {
                    return Ok(response);
                }
                return BadRequest("Object Not found!");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Unauthorized: {ex.Message}");
                return Unauthorized();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred.");
            }

        }
        [HttpGet]
        [Route("ESMAlias")]
        public async Task<IActionResult> GetESMAlias()
        {
            var token = _userService.GetToken();

            try
            {
                var responseInfo = await _smsService.GetESMSettings(token);

                if (responseInfo.ErrorCode == 0)
                {

                    return Ok(responseInfo.EsmAliasList);
                }

                return BadRequest(responseInfo.ErrorMessage);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Unauthorized: {ex.Message}");
                return Unauthorized();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        //DELETE
        [HttpDelete]
        [Route("DeleteSmsCampaignById/{id}")]
        public async Task<IActionResult> DeleteSmsCampaignById(int id)
        {
            var token = _userService.GetToken();

            try
            {
                var response = await _smsService.DeleteSmsCampaign(token, id);

                if (response.ErrorCode == 0)
                {
                    return Ok(response);
                }

                return BadRequest(response.ErrorMessage);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Unauthorized: {ex.Message}");
                return Unauthorized();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpGet]
        [Route("NumbersCount")]
        public async Task<IActionResult> GetNumbersCount(string phoneList, string clientId)
        {
            var token = _userService.GetToken();
            try
            {
                clientId = "493";
                var contacts = await _contactService.GetNumbersFromContactListAsync(token,clientId);
                

                return Ok();
            }
            catch (Exception)
            {
                return Ok(0);
            }
        }

        string ExtractPhones(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var phoneRegex = new Regex(@"^\+?[\d\s\-\(\)]{5,}$");

            var parts = input.Split(',')
                             .Select(p => p.Trim());

            var phones = parts.Where(p => phoneRegex.IsMatch(p));

            if (!phones.Any())
                return string.Empty;

            return string.Join(", ", phones);
        }

        string MergePhoneStrings(string s1, string s2)
        {
            var phones1 = s1.Split(',')
                            .Select(e => e.Trim())
                            .Where(e => !string.IsNullOrEmpty(e));

            var phones2 = s2.Split(',')
                            .Select(e => e.Trim())
                            .Where(e => !string.IsNullOrEmpty(e));

            var allPhones = phones1.Union(phones2);

            return string.Join(", ", allPhones);
        }

    }
}
