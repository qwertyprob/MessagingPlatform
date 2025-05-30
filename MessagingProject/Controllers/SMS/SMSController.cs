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
        [HttpGet]
        [Route("SmsCampaigns")]
        public async Task<IActionResult> SmsCampaigns()
        {
            var token = _userService.GetToken();
            var responseInfo = await _smsService.GetSmsGampaign(token);

            return Ok(responseInfo.CampaignList);
        }
    }
}
