using FluentValidation;
using FluentValidation.AspNetCore;
using MessagingProject.Abstractions;
using MessagingProject.Models.Auth;
using MessagingProject.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace MessagingProject.Controllers.Profile
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IValidator<LoginViewModel> _validator;

        public ProfileController(IAuthService authService, IValidator<LoginViewModel> validator, IUserService userService) 
        {
            _authService = authService;
            _validator = validator;
            _userService = userService;
        }
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
            var ClaimDict = GetUserClaims();

            return View(ClaimDict);
        }
        [AllowAnonymous]
        [Route("/ScreenLock")]

        public IActionResult ScreenLock()
        {
            var claims = GetUserClaims();

            HttpContext.Session.SetString("Email", claims["Email"]);
            HttpContext.Session.SetString("FullName", claims["FullName"]); 

            HttpContext.SignOutAsync();

            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.FullName = HttpContext.Session.GetString("FullName");

            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("/ScreenLock")]
        public async Task<IActionResult> LoginUser([FromForm] LoginViewModel model)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(model);
                if (validationResult.IsValid)
                {
                    var authResponse = await _authService.AuthenticateUserAsync(model);
                    await _authService.SignInUserAsync(authResponse, model);
                    return Redirect("/");

                }
                ViewBag.Claims = GetUserClaims();
                var claims = GetUserClaims();
                HttpContext.Session.SetString("Email", claims["Email"]);
                validationResult.AddToModelState(ModelState);


                return View("ScreenLock", model);


            }
            catch (UnauthorizedAccessException)
            {
                var errorMessage = $"* {Resources.ValidateResource.Invalid}";
                ViewData.ModelState.AddModelError("Email", errorMessage);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        private Dictionary<string, string> GetUserClaims()
        {
            return User.Claims.ToDictionary(c => c.Type, c => c.Value);
        }

        [AllowAnonymous]
        private IDictionary<string,string> GetUserClaimss()
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
