using FluentValidation;
using FluentValidation.AspNetCore;
using MessagingProject.Abstractions;
using MessagingProject.Models.Auth;
using MessagingProject.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace MessagingProject.Controllers.Profile
{
    [Route("Profile")]
    public class ProfileController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IValidator<LoginViewModel> _validator;

        public ProfileController(IAuthService authService, IValidator<LoginViewModel> validator, IUserService userService):base(authService) 
        {
            _authService = authService;
            _validator = validator;
            _userService = userService;
        }
        [Authorize]
        [Route("/EmailSignature")]
        public IActionResult EmailSignature()
        {
            return View();
        }
        [Authorize]
        [Route("/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [Authorize]
        [Route("/Profile")]
        public IActionResult ProfileInfo()
        {
            var ClaimDict = GetUserClaimsFiltered();

            if(ClaimDict is null)
            {
                return View(new Dictionary<string, string>());
            }
            return View(ClaimDict);
        }

        [AllowAnonymous]
        [Route("/ScreenLock")]
        public IActionResult ScreenLock()
        {

          HttpContext.SignOutAsync();


            var claims = GetUserClaims();


            ViewBag.Claims = claims;


            HttpContext.Session.SetString("Email", claims["Email"]);


            ViewBag.Email = HttpContext.Session.GetString("Email");


            return View();


        }



        private Dictionary<string, string> GetUserClaims()
        {
            return User.Claims.ToDictionary(c => c.Type, c => c.Value);
        }

        [AllowAnonymous]
        private IDictionary<string,string> GetUserClaimsFiltered()
        {
            var claims = HttpContext.User.Claims;

            var firstname = claims.FirstOrDefault(x => x.Type == "FirstName")?.Value ?? string.Empty;
            var lastname = claims.FirstOrDefault(x => x.Type == "Surname")?.Value ?? string.Empty;
            var phone = claims.FirstOrDefault(x => x.Type == "Phone")?.Value ?? string.Empty;
            var email = claims.FirstOrDefault(x => x.Type == "Email")?.Value ?? string.Empty;

            return new Dictionary<string, string>()
            {
                {  "FirstName", firstname  },
                {  "LastName", lastname  },
                {  "Phone", phone  },
                {  "Email", email  },
            };

        }



    }
}
