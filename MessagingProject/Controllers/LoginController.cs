using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers
{
    public class LoginController : Controller
    {
        [Route("Auth/Login")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Auth/AuthRecoverPassword")]
        public IActionResult AuthRecoverPassword()
        {
            return View();
        }
    }
}
