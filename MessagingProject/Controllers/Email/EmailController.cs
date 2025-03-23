using MessagingProject.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers.Email
{

    public class EmailController : BaseController
    {
        public EmailController(IAuthService auth):base(auth)
        {
            
        }
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
