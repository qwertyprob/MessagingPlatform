using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers
{
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
