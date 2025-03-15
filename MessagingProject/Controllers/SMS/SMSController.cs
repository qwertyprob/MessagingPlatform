using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers.SMS
{
    [Route("SMS")]

    public class SMSController : Controller
    {
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
