using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers
{
    public class ProfileController : Controller
    {

        [Route("/ProfileInfo")]
        public IActionResult ProfileInfo()
        {
            return View();
        }
    }
}
