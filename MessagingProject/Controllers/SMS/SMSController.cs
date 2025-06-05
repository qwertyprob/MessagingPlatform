using MessagingProject.Abstractions;
using MessagingProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers.SMS
{
    [Route("SMS")]

    [Authorize]
    public class SMSController : BaseController
    {
        readonly IAuthService _authService;
        readonly IUserService _userService;
        readonly ISMSService _smsService;
        public SMSController(IAuthService authService, IUserService userService, ISMSService smsService) : base(authService)
        {
            _authService = authService;
            _userService = userService;
            _smsService = smsService;
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

    }
}
