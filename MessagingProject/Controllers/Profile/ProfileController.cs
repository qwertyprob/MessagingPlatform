using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

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
            var claims = HttpContext.User.Claims;

            var firstname = claims.FirstOrDefault(x => x.Type == "FirstName")?.Value ?? string.Empty;
            var lastname = claims.FirstOrDefault(x => x.Type == "Surname")?.Value ?? string.Empty;
            var phone = claims.FirstOrDefault(x => x.Type == "Phone")?.Value ?? string.Empty;
            var email = claims.FirstOrDefault(x => x.Type == "Email")?.Value ?? string.Empty;

            var ClaimDict = new Dictionary<string, string>()
            {
                {  "FirstName", firstname  },
                {  "LastName", lastname  },
                {  "Phone", phone  },
                {  "Email", email  },
            };
            return View(ClaimDict);
        }

        [Route("/ScreenLock")]
        public IActionResult ScreenLock()
        {
            return View();
        }



    }
}
