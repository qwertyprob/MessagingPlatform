using MessagingProject.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers.SMS
{
    [Route("SMS")]

    [Authorize]
    public class SMSController : BaseController
    {
        public SMSController(IAuthService authService) : base(authService)
        {
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
    }
}
