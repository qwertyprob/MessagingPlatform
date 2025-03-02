using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers
{
    [Route("SMS")]
    [Authorize]
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
