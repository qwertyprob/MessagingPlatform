using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {
        [Route("Email/MailingList")]
        public IActionResult MailingList()
        {
            return View();
        }
        [Route("Email/SendEmail")]
        public IActionResult SendEmail()
        {
            return View();
        }
        [Route("Email/TemplateList")]
        public IActionResult TemplateList()
        {
            return View();
        }
    }
}
