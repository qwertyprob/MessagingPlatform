using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers.Authorization
{
    [AllowAnonymous]
    public class ProfileController : Controller
    {

        [Route("/ProfileInfo")]
        public IActionResult ProfileInfo()
        {
            return View();
        }
    }
}
