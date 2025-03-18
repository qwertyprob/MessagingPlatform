using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers.Profile
{
   
    public class ProfileController : Controller
    {
        [Route("/EmailSignature")]
        public IActionResult EmailSignature()
        {
            return View();
        }

        [Route("/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [Route("/Profile")]
        public IActionResult ProfileInfo()
        {
            return View();
        }

        [Route("/ScreenLock")]
        public IActionResult ScreenLock()
        {
            return View();
        }



    }
}
